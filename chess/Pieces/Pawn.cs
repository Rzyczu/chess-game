using chess.Enums;

namespace chess.Pieces
{
    public class Pawn : Piece
    {
        public bool IsMoved { get; set; }
        public bool IsPromoted { get; set; }
        public Pawn(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.Pawn)
        {
            IsMoved = false;
            IsPromoted = false;
        }
        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            // Check if end coordinates are within the board bounds
            if (!board.IsWithinBounds(end))
            {
                return false;
            }

            int deltaX = end.X - start.X;
            int deltaY = Math.Abs(end.Y - start.Y);

            if (Player.Color == ColorType.White)
            {
                // White pawn moves
                return IsValidWhitePawnMove(deltaX, deltaY, start, end, board);
            }
            else
            {
                // Black pawn moves
                return IsValidBlackPawnMove(deltaX, deltaY, start, end, board);
            }
        }

        private bool IsValidWhitePawnMove(int deltaX, int deltaY, Coordinates start, Coordinates end, GameBoard board)
        {
            if (deltaX == -1 && deltaY == 0 && board.GetPieceAt(end) == null)
            {
                return true; // Move one square forward
            }
            if (deltaX == -2 && deltaY == 0 && start.X == 6 && board.GetPieceAt(end) == null && board.GetPieceAt(new Coordinates(5, end.Y)) == null)
            {
                return true; // Move two squares forward from the initial position
            }
            if (deltaX == -1 && deltaY == 1 && board.GetPieceAt(end) != null && board.GetPieceAt(end).Player.Color == ColorType.Black)
            {
                return true; // Capture diagonally
            }
            return false;
        }

        private bool IsValidBlackPawnMove(int deltaX, int deltaY, Coordinates start, Coordinates end, GameBoard board)
        {
            if (deltaX == 1 && deltaY == 0 && board.GetPieceAt(end) == null)
            {
                return true; // Move one square forward
            }
            if (deltaX == 2 && deltaY == 0 && start.X == 1 && board.GetPieceAt(end) == null && board.GetPieceAt(new Coordinates(2, end.Y)) == null)
            {
                return true; // Move two squares forward from the initial position
            }
            if (deltaX == 1 && deltaY == 1 && board.GetPieceAt(end) != null && board.GetPieceAt(end).Player.Color == ColorType.White)
            {
                return true; // Capture diagonally
            }
            return false;
        }


    }
}
