using chess.Pieces;
using chess;
using chess.Enums;
using System.Runtime.CompilerServices;

public class Game
{
    private GameBoard board;
    private Player player1;
    private Player player2;
    private List<Turn> turnsHistory;
    private Turn currentTurn;

    public Game()
    {
        // Initialize players
        player1 = new Player(ColorType.White);
        player2 = new Player(ColorType.Black);

        // Initialize game board
        board = new GameBoard(8, 8);

        // Set up initial pieces
        SetInitialPieces();

        // Initialize turns history
        turnsHistory = new List<Turn>();

        // Start the game with player 1 (white) making the first move
        currentTurn = new Turn(1, player1);
    }

    private void SetInitialPieces()
    {
        // Set up white pieces
        board.AddPiece(new Rook(new Coordinates(0, 0), player1), new Coordinates(0, 0));
        board.AddPiece(new Knight(new Coordinates(0, 1), player1), new Coordinates(0, 1));
        board.AddPiece(new Bishop(new Coordinates(0, 2), player1), new Coordinates(0, 2));
        board.AddPiece(new Queen(new Coordinates(0, 3), player1), new Coordinates(0, 3));
        board.AddPiece(new King(new Coordinates(0, 4), player1), new Coordinates(0, 4));
        board.AddPiece(new Bishop(new Coordinates(0, 5), player1), new Coordinates(0, 5));
        board.AddPiece(new Knight(new Coordinates(0, 6), player1), new Coordinates(0, 6));
        board.AddPiece(new Rook(new Coordinates(0, 7), player1), new Coordinates(0, 7));

        for (int i = 0; i < 8; i++)
        {
            board.AddPiece(new Pawn(new Coordinates(1, i), player1), new Coordinates(1, i));
        }

        // Set up black pieces
        board.AddPiece(new Rook(new Coordinates(7, 0), player2), new Coordinates(7, 0));
        board.AddPiece(new Knight(new Coordinates(7, 1), player2), new Coordinates(7, 1));
        board.AddPiece(new Bishop(new Coordinates(7, 2), player2), new Coordinates(7, 2));
        board.AddPiece(new Queen(new Coordinates(7, 3), player2), new Coordinates(7, 3));
        board.AddPiece(new King(new Coordinates(7, 4), player2), new Coordinates(7, 4));
        board.AddPiece(new Bishop(new Coordinates(7, 5), player2), new Coordinates(7, 5));
        board.AddPiece(new Knight(new Coordinates(7, 6), player2), new Coordinates(7, 6));
        board.AddPiece(new Rook(new Coordinates(7, 7), player2), new Coordinates(7, 7));

        for (int i = 0; i < 8; i++)
        {
            board.AddPiece(new Pawn(new Coordinates(6, i), player2), new Coordinates(6, i));
        }
    }


    public void StartGame()
    {
        Console.WriteLine("Welcome! Starting new game of chess...\n\n\n");
        Console.WriteLine($"Turn: {currentTurn.Number}\n");
        PrintBoard();
        PlayGame();
    }

    private void PlayGame()
    {
        // Placeholder for game loop
        while (!IsGameOver())
        {
            // Execute player's turn
            ExecuteTurn();
            // Print current board state
            Console.WriteLine($"\n------------------------------------------\n");
            Console.WriteLine($"Turn: {currentTurn.Number}\n");
            PrintBoard();
            // Check if game over condition is met
            if (IsGameOver())
            {
                Console.WriteLine("Game over!");
                // Print winner or draw
                PrintResult();
            }
        }
    }

    private void ExecuteTurn()
    {
        Console.WriteLine($"\nPlayer {currentTurn.Player.Color}'s turn\n");

        MakeMove();
        UpdateTurn();
    }

    private void MakeMove()
    {
        Console.WriteLine("Enter your move (e.g., 'e2 e4'):");
        string moveInput = Console.ReadLine();
        string[] moveParts = moveInput.Split(' ');

        if (moveParts.Length != 2)
        {
            Console.WriteLine("Invalid move format.");
            MakeMove(); // Retry move
            return;
        }

        string startPosition = moveParts[0];
        string endPosition = moveParts[1];

        // Convert algebraic notation to board coordinates (e.g., 'e2' -> (1, 4))
        Coordinates startCoord = AlgebraicToCoordinates(startPosition);
        Coordinates endCoord = AlgebraicToCoordinates(endPosition);

        if (!board.IsWithinBounds(startCoord) || !board.IsWithinBounds(endCoord))
        {
            Console.WriteLine("Position is out of bounds.");
            MakeMove(); // Retry move
            return;
        }

        Piece pieceAtStart = board.GetPieceAt(startCoord);

        if (pieceAtStart == null)
        {
            Console.WriteLine("No piece at start position.");
            MakeMove(); // Retry move
            return;
        }

        Move move = new Move(startCoord, endCoord, pieceAtStart, null);
        if (!IsValidMove(move))
        {
            Console.WriteLine("Invalid move. Please try again.");
            MakeMove(); // Retry move
            return;
        }

        currentTurn.Move = move;
        ExecuteMove(move);

        // Check for castling
        if (pieceAtStart is King && Math.Abs(startCoord.Y - endCoord.Y) == 2)
        {
            // Castling
            HandleCastling(startCoord, endCoord);
        }
    }

    private void HandleCastling(Coordinates startCoord, Coordinates endCoord)
    {
        // Get the pieces
        Piece king = board.GetPieceAt(startCoord);
        Piece rook = board.GetPieceAt(new Coordinates(startCoord.X, (endCoord.Y == 6) ? 7 : 0)); // Determine rook's start position

        // Check if both the king and the rook haven't moved
        if (!(king is King kingPiece && !kingPiece.IsMoved) || !(rook is Rook rookPiece && !rookPiece.IsMoved))
        {
            Console.WriteLine("Invalid castling: King or rook has already moved.");
            return;
        }

        // Check if there are any pieces between the king and the rook
        int rookEndY = (endCoord.Y == 6) ? 5 : 3;
        for (int col = Math.Min(startCoord.Y, rookEndY) + 1; col < Math.Max(startCoord.Y, rookEndY); col++)
        {
            if (board.GetPieceAt(new Coordinates(startCoord.X, col)) != null)
            {
                Console.WriteLine("Invalid castling: Pieces obstruct the path.");
                return;
            }
        }

        // Move the rook
        Coordinates rookStart = new Coordinates(startCoord.X, (endCoord.Y == 6) ? 7 : 0);
        Coordinates rookEnd = new Coordinates(startCoord.X, rookEndY);
        board.RemovePieceAt(rookStart);
        board.AddPiece(rook, rookEnd);
        rook.Coordinates = rookEnd;
        ((Rook)rook).IsMoved = true;

        // Move the king
        board.RemovePieceAt(startCoord);
        board.AddPiece(king, endCoord);
        king.Coordinates = endCoord;
        ((King)king).IsMoved = true;
    }


    private void ExecuteMove(Move move)
    {
        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;
        Piece pieceAtStart = move.PiecePlayed;

        // Check if a piece is being captured
        Piece pieceAtEnd = board.GetPieceAt(end);

        if (pieceAtEnd != null && pieceAtEnd.Player != pieceAtStart.Player)
        {
            board.RemovePieceAt(end);
            // Increment the capturing player's score
            currentTurn.Player.Score++;
        }

        if (pieceAtStart.Type == PieceType.Pawn)
        {
            if (!((Pawn)pieceAtStart).IsMoved)
            {
                ((Pawn)pieceAtStart).IsMoved = true;
            }

        }

        // Move the piece to the new position
        board.RemovePieceAt(start);
        board.AddPiece(pieceAtStart, end);

        // Update the piece's coordinates
        pieceAtStart.Coordinates = end;

        turnsHistory.Add(currentTurn);
        // Additional move-related logic can be added here
    }



    private Coordinates AlgebraicToCoordinates(string algebraicNotation)
    {
        int row = algebraicNotation[1] - '1';
        int col = algebraicNotation[0] - 'a';
        return new Coordinates(row, col);
    }

    private void UpdateTurn()
    {
        currentTurn = new Turn(currentTurn.Number + 1, (currentTurn.Player == player1) ? player2 : player1);
    }

    private bool IsValidMove(Move move)
    {
        Piece piece = move.PiecePlayed;

        if (piece == null || piece.Player != currentTurn.Player)
        {
            Console.WriteLine($"Start position doesn't contain {currentTurn.Player.Color} piece.");
            return false;
        }

        Coordinates start = move.StartPosition;
        Coordinates end = move.EndPosition;

        Piece pieceAtEnd = board.GetPieceAt(end);
        if (pieceAtEnd != null && pieceAtEnd.Player == currentTurn.Player)
        {
            return false;
        }

        if (!piece.IsValidMove(start, end, board))
        {
            return false;
        }

        if (!(piece is Knight) && !IsPathClear(start, end, board))
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
            // Check if the current position is occupied by a piece
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
        // Placeholder for game over condition
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
            Console.Write(columnLabel.ToString().PadLeft(2).PadRight(3));
        }
        Console.WriteLine();

        for (int row = 0; row < board.Height; row++)
        {
            Console.ForegroundColor = darkSquareColor;
            Console.Write((row + 1).ToString().PadLeft(2) + " ");

            ConsoleColor squareColor = (row % 2 == 0) ? darkSquareColor : lightSquareColor;
            for (int col = 0; col < board.Width; col++)
            {
                Console.BackgroundColor = squareColor;

                Piece piece = board.GetPieceAt(new Coordinates(row, col));
                if (piece != null)
                {
                    ConsoleColor pieceColor = (piece.Player.Color == ColorType.White) ? whiteColor : blackColor;
                    Console.ForegroundColor = pieceColor;
                    Console.Write(piece.Type.ToString().Substring(0, 1).PadLeft(2).PadRight(3));
                }
                else
                {
                    Console.Write(" ".PadRight(3));
                }

                squareColor = (squareColor == darkSquareColor) ? lightSquareColor : darkSquareColor;
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }


    private void PrintResult()
    {
        // Placeholder for printing game result
        Console.WriteLine("Printing game result...");
    }



}
