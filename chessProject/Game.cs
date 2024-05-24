using chess.Pieces;
using chess.Enums;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Net.NetworkInformation;
using System.Text;
using chess.Helpers;

namespace chess
{
    public class Game
    {
        private readonly GameBoard board;
        private readonly Player player1;
        private readonly Player player2;
        private readonly List<Turn> turnsHistory;
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
                ConsoleHelper.WriteInfo(InfoMessages.CurrentTurnInfo(currentTurn));
                board.PrintBoard();
                ConsoleHelper.WriteInfo(InfoMessages.CurrentPlayerInfo(currentTurn));
                CheckKingInCheck(player1);
                CheckKingInCheck(player2);
                ExecuteTurn();

                if (IsGameOver())
                {
                    ConsoleHelper.WriteInfo(InfoMessages.GameOverInfo);
                    PrintResult();
                }
            }
        }

        private void CheckKingInCheck(Player player)
        {
            King king = (King)board.GetPieceOfType(PieceType.King, player);
            if (IsKingInCheck(player))
            {
                king.IsInCheck = true;

                ConsoleHelper.WriteWarning(WarningMessages.KingInCheckWarning(currentTurn));
                Console.WriteLine();
            }
            else
            {
                king.IsInCheck = false;
            }
        }

        private void ExecuteTurn()
        {
            MakeMove();
            UpdateTurn();
        }

        private void MakeMove()
        {
            ConsoleHelper.WriteInfo(InfoMessages.EnterMoveInfo);
            Console.ForegroundColor = ConsoleColor.Cyan;
            string ?moveInput = Console.ReadLine();
            Console.ResetColor();
            string[] moveParts = moveInput.Split(' ');

            if (moveParts.Length != 2 || !IsValidInput(moveParts[0]) || !IsValidInput(moveParts[1]))
            {
                ConsoleHelper.WriteError(ErrorMessages.MoveFormatInputError);
                MakeMove();
                return;
            }

            string startPosition = moveParts[0];
            string endPosition = moveParts[1];

            Coordinates start = AlgebraicToCoordinates(startPosition);
            Coordinates end = AlgebraicToCoordinates(endPosition);

            if (!board.IsWithinBounds(start) || !board.IsWithinBounds(end))
            {
                ConsoleHelper.WriteError(ErrorMessages.WithinBoundError);
                MakeMove(); // Retry move
                return;
            }

            Piece pieceAtStart = board.GetPieceAt(start);

            if (pieceAtStart == null)
            {
                ConsoleHelper.WriteError(ErrorMessages.PieceStartError);
                MakeMove(); // Retry move
                return;
            }

            Move move = new Move(start, end, pieceAtStart, null);

            if (!IsValidMove(move))
            {
                ConsoleHelper.WriteError(ErrorMessages.InvalidMoveError);
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
            int rookX = end.X + (deltaX == 1 ? 1 : deltaX == -1 ? -2 : 0);

            Piece rook = board.GetPieceAt(new Coordinates(rookX, start.Y));

            // Check if both the king and the rook haven't moved
            if (!(king is King kingPiece && !kingPiece.IsMoved) || !(rook is Rook rookPiece && !rookPiece.IsMoved))
            {
                ConsoleHelper.WriteError(ErrorMessages.CatlingPieceMovedError);
                return false;
            }

            // Check if there are any pieces between the king and the rook
            int rookEndX = rookX - deltaX;
            for (int row = Math.Min(start.X, rookEndX) + 1; row < Math.Max(start.X, rookEndX); row++)
            {
                if (board.GetPieceAt(new Coordinates(row, start.Y)) != null)
                {
                    ConsoleHelper.WriteError(ErrorMessages.CastlingPacthError);
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
            int rookX = end.X + (deltaX == 1 ? 1 : deltaX == -1 ? -2 : 0);

            Piece rook = board.GetPieceAt(new Coordinates(rookX, start.Y));
            int rookEndX = rookX - deltaX;

            // Move the rook
            Coordinates rookStart = new Coordinates(end.X == 6 ? 7 : 0, start.Y);
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
            Piece ?pieceAtEnd = board.GetPieceAt(end);

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

            if (pieceAtStart.Type == PieceType.Pawn &&
                 (currentTurn.Player.Color ==
                 ColorType.White ? pieceAtStart.Coordinates.Y == 8 : pieceAtStart.Coordinates.Y == 0))
            {
                PromotePawn(pieceAtStart);
            }
                turnsHistory.Add(currentTurn);
        }

        private void PromotePawn(Piece pieceBeforePromotion)
        {
            Console.WriteLine();
            ConsoleHelper.WriteInfo(InfoMessages.PromotePawnInfo);
            ConsoleHelper.WriteInfo(InfoMessages.PromotePawnOptions);


            string? charPiece = Console.ReadLine();
            int pieceChoice;

            // Walidacja wejścia
            while (!int.TryParse(charPiece, out pieceChoice) || pieceChoice < 1 || pieceChoice > 4)
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4:");
                charPiece = Console.ReadLine();
            }

            board.RemovePieceAt(pieceBeforePromotion.Coordinates);

            Piece pieceAfterPromotion;

            // Instrukcja switch
            switch (pieceChoice)
            {
                case 1:
                    pieceAfterPromotion = new Queen(pieceBeforePromotion.Coordinates, currentTurn.Player);
                    break;
                case 2:
                    pieceAfterPromotion = new Rook(pieceBeforePromotion.Coordinates, currentTurn.Player);
                    break;
                case 3:
                    pieceAfterPromotion = new Bishop(pieceBeforePromotion.Coordinates, currentTurn.Player);
                    break;
                case 4:
                    pieceAfterPromotion = new Knight(pieceBeforePromotion.Coordinates, currentTurn.Player);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value for promotion.");
            }

            currentTurn.Move.Promotion = pieceAfterPromotion;

            board.AddPiece(pieceAfterPromotion,pieceAfterPromotion.Coordinates);
        }

        private Coordinates AlgebraicToCoordinates(string algebraicNotation)
        {
            int row = algebraicNotation[0] - 'a';
            int col = algebraicNotation[1] - '1';
            return new Coordinates(row, col);
        }

        private void UpdateTurn()
        {
            currentTurn = new Turn(currentTurn.Number + 1, currentTurn.Player == player1 ? player2 : player1);
        }

        private bool IsValidMove(Move move)
        {
            Piece piece = move.PiecePlayed;

            //Enemy piece is on start position
            if (piece == null || piece.Player != currentTurn.Player)
            {
                ConsoleHelper.WriteError(ErrorMessages.EnemyPieceStartError(currentTurn));
                return false;
            }

            Coordinates start = move.StartPosition;
            Coordinates end = move.EndPosition;

            //Ally piece is on end position
            Piece ?pieceAtEnd = board.GetPieceAt(end);
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
                ConsoleHelper.WriteError(ErrorMessages.InvalidPacthError);
                return false;
            }

            //Other piece moved when king is in check
            Piece currentPlayerKing = board.GetPieceOfType(PieceType.King, currentTurn.Player);
            if (((King)currentPlayerKing).IsInCheck && piece != currentPlayerKing)
            {
                ConsoleHelper.WriteError(ErrorMessages.NoKingInCheckMoveError);
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
            // Check if either player's king is in check and there are no possible moves to escape
            if (IsKingInCheck(player1) && !CanEscapeCheck(player1))
            {
                return true;
            }

            if (IsKingInCheck(player2) && !CanEscapeCheck(player2))
            {
                return true;
            }

            return false;
        }

        private bool CanEscapeCheck(Player player)
        {
            // Get all pieces of the current player
            List<Piece> playerPieces = player == player1 ? board.WhitePieces : board.BlackPieces;

            foreach (Piece piece in playerPieces)
            {
                Coordinates start = piece.Coordinates;
                // Try moving to every position on the board
                for (int x = 0; x < board.Width; x++)
                {
                    for (int y = 0; y < board.Height; y++)
                    {
                        Coordinates end = new Coordinates(x, y);
                        Piece ?pieceAtEnd = board.GetPieceAt(end);
                        Move move = new Move(start, end, piece, pieceAtEnd);
                        if (IsValidMove(move))
                        {
                            // Simulate the move
                            board.RemovePieceAt(start);
                            board.AddPiece(piece, end);
                            bool isInCheck = IsKingInCheck(player);
                            // Revert the move
                            board.AddPiece(piece, start);
                            board.RemovePieceAt(end);
                            if (pieceAtEnd != null)
                            {
                                board.AddPiece(pieceAtEnd, pieceAtEnd.Coordinates);
                            }
                            if (isInCheck == false)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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
            List<Piece> opponentPieces = player == player1 ? board.BlackPieces : board.WhitePieces;

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
            Console.WriteLine("Game results");
            Console.WriteLine("Game results");
            Player winner = IsKingInCheck(player1) ? player2 : player1;
            Console.WriteLine($"Winner: {winner.Color}");
            Console.WriteLine("\nMoves History:");
            foreach (var turn in turnsHistory)
            {
                Console.WriteLine($"{turn.Number}. {turn.Player.Color} {FormatMove(turn.Move)}");
            }
        }

        private string FormatMove(Move move)
        {
            StringBuilder moveNotation = new StringBuilder();

            moveNotation.Append(GetPieceNotation(move.PiecePlayed));
            moveNotation.Append(FormatCoordinates(move.StartPosition));
            moveNotation.Append(move.PieceCaptured != null ? "x" : "-");
            moveNotation.Append(FormatCoordinates(move.EndPosition));
            if (move.Promotion != null)
            {
                moveNotation.Append("=");
                moveNotation.Append(GetPieceNotation(move.Promotion));
            }

            return moveNotation.ToString();
        }

        private string FormatCoordinates(Coordinates coordinates)
        {
            char file = (char)('a' + coordinates.X);
            char rank = (char)('1' + coordinates.Y);
            return $"{file}{rank}";
        }

        private string GetPieceNotation(Piece piece)
        {
            return piece.Type switch
            {
                PieceType.King => "K",
                PieceType.Queen => "Q",
                PieceType.Rook => "R",
                PieceType.Bishop => "B",
                PieceType.Knight => "N",
                PieceType.Pawn => "",
                _ => ""
            };
        }
    }
}