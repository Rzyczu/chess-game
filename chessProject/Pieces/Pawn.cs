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

            int deltaX = Math.Abs(end.X - start.X);
            int deltaY = end.Y - start.Y;

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
            if (deltaY == 1 && deltaX == 0 && board.GetPieceAt(end) == null)
            {
                return true; // Move one square forward
            }
            if (deltaY == 2 && deltaX == 0  && board.GetPieceAt(end) == null && !IsMoved)
            {
                return true; // Move two squares forward from the initial position
            }
            if (deltaY == 1 && deltaX == 1 && board.GetPieceAt(end) != null && board.GetPieceAt(end).Player.Color == ColorType.Black)
            {
                return true; // Capture diagonally
            }
            return false;
        }

        private bool IsValidBlackPawnMove(int deltaX, int deltaY, Coordinates start, Coordinates end, GameBoard board)
        {
            if (deltaY == -1 && deltaX == 0 && board.GetPieceAt(end) == null)
            {
                return true; // Move one square forward
            }
            if (deltaY == -2 && deltaX == 0 && board.GetPieceAt(end) == null && !IsMoved)
            {
                return true; // Move two squares forward from the initial position
            }
            if (deltaY == -1 && deltaX == 1 && board.GetPieceAt(end) != null && board.GetPieceAt(end).Player.Color == ColorType.White)
            {
                return true; // Capture diagonally
            }
            return false;
        }


    }
}
