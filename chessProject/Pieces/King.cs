using chess.Enums;

namespace chess.Pieces
{
    public class King : Piece
    {
        public bool IsMoved { get; set; }
        public bool IsCastled { get; set; }
        public bool IsInCheck { get; set; }
        public King(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.King)
        {
            IsMoved = false;
            IsCastled = false;
            IsInCheck = false;
        }

        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            int deltaX = Math.Abs(start.X - end.X);
            int deltaY = Math.Abs(start.Y - end.Y);

            // King can move one square in any direction
            if (deltaX <= 1 && deltaY <= 1)
            {
                return true;
            }
            // King can castling
            if (deltaX == 2)
            {
                return true;
            }

            return false;
        }
    }
}
