using chess.Pieces;
using chess;
using chess.Enums;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Net.NetworkInformation;
using System.Text;

public class Game
{
    private GameBoard board;
    private Player player1;
    private Player player2;
    private List<Turn> turnsHistory;
    private Turn currentTurn;

    public Game()
    {
        player1 = new Player(ColorType.White);
        player2 = new Player(ColorType.Black);
        board = new GameBoard(8, 8);
        SetInitialPieces();
        turnsHistory = new List<Turn>();
        currentTurn = new Turn(1, player1);
    }

    private void SetInitialPieces()
    {
        PlacePiecesForPlayer(player1, 0);
        PlacePiecesForPlayer(player2, 7);

        for (int row = 0; row < board.Width; row++)
        {
            board.AddPiece(new Pawn(new Coordinates(row, 1), player1), new Coordinates(row, 1));
            board.AddPiece(new Pawn(new Coordinates(row, board.Height - 2), player2), new Coordinates(row, 6));
        }
    }

    private void PlacePiecesForPlayer(Player player, int col)
    {
        // Place Rooks
        board.AddPiece(new Rook(new Coordinates(0, col), player), new Coordinates(0, col));
        board.AddPiece(new Rook(new Coordinates(7, col), player), new Coordinates(7, col));
        // Place Knights
        board.AddPiece(new Knight(new Coordinates(1, col), player), new Coordinates(1, col));
        board.AddPiece(new Knight(new Coordinates(6, col), player), new Coordinates(6, col));
        // Place Bishops
        board.AddPiece(new Bishop(new Coordinates(2, col), player), new Coordinates(2, col));
        board.AddPiece(new Bishop(new Coordinates(5, col), player), new Coordinates(5, col));
        // Place Queen
        board.AddPiece(new Queen(new Coordinates(3, col), player), new Coordinates(3, col));
        // Place King
        board.AddPiece(new King(new Coordinates(4, col), player), new Coordinates(4, col));
    }

    public void StartGame()
    {
        Console.WriteLine("Welcome! Starting new game of chess...\n\n");
        PlayGame();
    }

    private void PlayGame()
    {
        while (!IsGameOver())
        {
            Console.WriteLine($"\n------------------------------------------\n");
            Console.WriteLine($"Turn: {currentTurn.Number} \n");

            PrintBoard();
            Console.WriteLine($"Player {currentTurn.Player.Color} move \n");

            CheckKingInCheck(player1);
            CheckKingInCheck(player2);

            ExecuteTurn();

            if (IsGameOver())
            {
                Console.WriteLine("Game over!");
                PrintResult();
            }
        }
    }

    private void CheckKingInCheck(Player player)
    {
        King king = ((King)board.GetPieceOfType(PieceType.King, player));
        if (IsKingInCheck(player))
        {
            king.IsInCheck = true;
            Console.WriteLine($"{player.Color} King is in check\n");
        }
        else
        {
            king.IsInCheck = false;
        }
    }

    private bool IsInCheck()
    {
        if (IsKingInCheck(player1))
        {
            return true;
        }

        return false;
    }

    private void ExecuteTurn()
    {
        MakeMove();
        UpdateTurn();
    }

    private void MakeMove()
    {
        Console.WriteLine("Enter your move (e.g., 'e2 e4'):");
        string moveInput = Console.ReadLine();
        string[] moveParts = moveInput.Split(' ');

        if (moveParts.Length != 2 || !IsValidInput(moveParts[0]) || !IsValidInput(moveParts[1]))
        {
            Console.WriteLine(ErrorMessages.MoveFormatInputError);
            MakeMove();
            return;
        }

        string startPosition = moveParts[0];
        string endPosition = moveParts[1];

        Coordinates start = AlgebraicToCoordinates(startPosition);
        Coordinates end = AlgebraicToCoordinates(endPosition);

        if (!board.IsWithinBounds(start) || !board.IsWithinBounds(end))
        {
            Console.WriteLine(ErrorMessages.WithinBoundError);
            MakeMove(); // Retry move
            return;
        }

        Piece pieceAtStart = board.GetPieceAt(start);

        if (pieceAtStart == null)
        {
            Console.WriteLine(ErrorMessages.PieceStartError);
            MakeMove(); // Retry move
            return;
        }

        Move move = new Move(start, end, pieceAtStart, null);

        if (!IsValidMove(move))
        {
            Console.WriteLine(ErrorMessages.InvalidMoveError);
            MakeMove(); // Retry move
            return;
        }

        currentTurn.Move = move;

        // Check for castling
        if (pieceAtStart is King && Math.Abs(start.X - end.X) == 2)
        {
            if (IsValidCastling(move))
            {
                HandleCastling(move);
            }
            else
            {
                MakeMove(); // Retry move
                return;
            }
        }
        ExecuteMove(move);
    }

    private bool IsValidInput(string position)
    {
        return position.Length == 2 &&
               position[0] >= 'a' && position[0] <= 'h' &&
               position[1] >= '1' && position[1] <= '8';
    }

    private bool IsValidCastling(Move move)
    {

        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;
        Piece king = move.PiecePlayed;

        int deltaX = Math.Sign(end.X - start.X);
        int rookX = end.X + (deltaX == 1 ? 1 : (deltaX == -1 ? -2 : 0));

        Piece rook = board.GetPieceAt(new Coordinates(rookX, start.Y));

        // Check if both the king and the rook haven't moved
        if (!(king is King kingPiece && !kingPiece.IsMoved) || !(rook is Rook rookPiece && !rookPiece.IsMoved))
        {
            Console.WriteLine(ErrorMessages.CatlingPieceMovedError);
            return false;
        }

        // Check if there are any pieces between the king and the rook
        int rookEndX = rookX - deltaX;
        for (int row = Math.Min(start.X, rookEndX) + 1; row < Math.Max(start.X, rookEndX); row++)
        {
            if (board.GetPieceAt(new Coordinates(row, start.Y)) != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ErrorMessages.CastlingPacthError);
                Console.ResetColor();
                return false;
            }
        }

        return true;

    }
    private void HandleCastling(Move move)
    {

        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;
        Piece king = move.PiecePlayed;
        int deltaX = Math.Sign(end.X - start.X);
        int rookX = end.X + (deltaX == 1 ? 1 : (deltaX == -1 ? -2 : 0));

        Piece rook = board.GetPieceAt(new Coordinates(rookX, start.Y));
        int rookEndX = rookX - deltaX;

        // Move the rook
        Coordinates rookStart = new Coordinates((end.X == 6) ? 7 : 0, start.Y);
        Coordinates rookEnd = new Coordinates(rookEndX, start.Y);
        board.RemovePieceAt(rookStart);
        board.AddPiece(rook, rookEnd);
        rook.Coordinates = rookEnd;
        ((Rook)rook).IsMoved = true;
        // Move the king
        board.RemovePieceAt(start);
        board.AddPiece(king, end);
        king.Coordinates = end;
        ((King)king).IsMoved = true;

        turnsHistory.Add(currentTurn);
    }

    private void ExecuteMove(Move move)
    {
        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;
        Piece pieceAtStart = move.PiecePlayed;
        Piece pieceAtEnd = board.GetPieceAt(end);


        if (pieceAtEnd != null && pieceAtEnd.Player != pieceAtStart.Player)
        {
            board.RemovePieceAt(end);
            currentTurn.Player.Score++;
        }

        if (pieceAtStart.Type == PieceType.Pawn)
        {
            if (!((Pawn)pieceAtStart).IsMoved)
            {
                ((Pawn)pieceAtStart).IsMoved = true;
            }
        }
        if (pieceAtStart.Type == PieceType.King)
        {
            if (!((King)pieceAtStart).IsMoved)
            {
                ((King)pieceAtStart).IsMoved = true;
            }
        }
        if (pieceAtStart.Type == PieceType.Rook)
        {
            if (!((Rook)pieceAtStart).IsMoved)
            {
                ((Rook)pieceAtStart).IsMoved = true;
            }
        }

        board.RemovePieceAt(start);
        board.AddPiece(pieceAtStart, end);

        pieceAtStart.Coordinates = end;

        turnsHistory.Add(currentTurn);
    }

    private Coordinates AlgebraicToCoordinates(string algebraicNotation)
    {
        int row = algebraicNotation[0] - 'a';
        int col = algebraicNotation[1] - '1';
        return new Coordinates(row, col);
    }

    private void UpdateTurn()
    {
        currentTurn = new Turn(currentTurn.Number + 1, (currentTurn.Player == player1) ? player2 : player1);
    }

    private bool IsValidMove(Move move)
    {
        Piece piece = move.PiecePlayed;

        //Enemy piece is on start position
        if (piece == null || piece.Player != currentTurn.Player)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorMessages.EnemyPieceStartError(currentTurn.Player));
            Console.ResetColor();
            return false;
        }

        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;

        //Ally piece is on end position
        Piece pieceAtEnd = board.GetPieceAt(end);
        if (pieceAtEnd != null && pieceAtEnd.Player == currentTurn.Player)
        {
            return false;
        }

        //Valid move
        if (!piece.IsValidMove(start, end, board))
        {
            return false;
        }

        //Patch is obstruck
        if (!(piece is Knight) && !IsPathClear(start, end, board))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorMessages.InvalidPacthError);
            Console.ResetColor();
            return false;
        }

        //Other piece moved when king is in check
        Piece currentPlayerKing = board.GetPieceOfType(PieceType.King, currentTurn.Player);
        if (((King)currentPlayerKing).IsInCheck && piece != currentPlayerKing)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorMessages.NoKingInCheckMoveError);
            Console.ResetColor();
            return false;
        }

        //Piece is move on king in check position
        bool prevKingState = ((King)currentPlayerKing).IsInCheck;
        board.RemovePieceAt(start);
        board.RemovePieceAt(end);
        board.AddPiece(piece, end);

        bool isInCheck = IsKingInCheck(currentTurn.Player);

        ((King)currentPlayerKing).IsInCheck = prevKingState;
        board.AddPiece(piece, start);
        board.RemovePieceAt(end);

        if (pieceAtEnd != null)
        {
            board.AddPiece(pieceAtEnd, end);
        }

        if (isInCheck)
        {
            return false;
        }


        return true;
    }

    private bool IsPathClear(Coordinates start, Coordinates end, GameBoard board)
    {
        int deltaX = Math.Sign(end.X - start.X);
        int deltaY = Math.Sign(end.Y - start.Y);

        int currentX = start.X + deltaX;
        int currentY = start.Y + deltaY;

        while (currentX != end.X || currentY != end.Y)
        {
            if (board.GetPieceAt(new Coordinates(currentX, currentY)) != null)
            {
                return false; // Path is blocked by another piece
            }

            currentX += deltaX;
            currentY += deltaY;
        }

        return true; // Path is clear
    }

    private bool IsGameOver()
    {
        return false;
    }

    private void PrintBoard()
    {
        ConsoleColor lightSquareColor = ConsoleColor.DarkGreen;
        ConsoleColor darkSquareColor = ConsoleColor.DarkGray;
        ConsoleColor whiteColor = ConsoleColor.White;
        ConsoleColor blackColor = ConsoleColor.Black;

        Console.Write("   ");
        for (int col = 0; col < board.Width; col++)
        {
            Console.ForegroundColor = darkSquareColor;
            char columnLabel = (char)('A' + col);
            Console.Write(columnLabel.ToString().PadRight(2));
        }
        Console.WriteLine();

        for (int row = 0; row < board.Height; row++)
        {
            Console.ForegroundColor = darkSquareColor;
            Console.Write((row + 1).ToString().PadRight(1) + " ");

            ConsoleColor squareColor = (row % 2 == 0) ? darkSquareColor : lightSquareColor;
            for (int col = 0; col < board.Width; col++)
            {
                Console.BackgroundColor = squareColor;

                Piece piece = board.GetPieceAt(new Coordinates(col, row));
                if (piece != null)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    ConsoleColor pieceColor = (piece.Player.Color == ColorType.White) ? whiteColor : blackColor;
                    Console.ForegroundColor = pieceColor;
                    char symbol = PieceSymbols.Symbols[piece.Type];
                    Console.Write(symbol.ToString().PadRight(2));
                }
                else
                {
                    Console.Write(" ".PadRight(2));
                }

                squareColor = (squareColor == darkSquareColor) ? lightSquareColor : darkSquareColor;
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine();

    }

    private bool IsKingInCheck(Player player)
    {
        Piece kingPiece = board.GetPieceOfType(PieceType.King, player);
        King? king = kingPiece as King;

        if (king == null)
        {
            throw new InvalidOperationException("King not found on the board.");
        }


        // Get all opponent pieces
        List<Piece> opponentPieces = (player == player1) ? board.BlackPieces : board.WhitePieces;

        foreach (Piece piece in opponentPieces)
        {
            if (piece.IsValidMove(piece.Coordinates, king.Coordinates, board) && IsPathClear(piece.Coordinates, king.Coordinates, board))
            {
                return true;
            }
        }
        return false;
    }


    private void PrintResult()
    {
        Console.WriteLine("Printing game result...");
    }



}