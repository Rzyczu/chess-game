using chess.Components;
using chess.Helpers.Enums;

namespace chess.Components.Pieces
{
    public class Rook : Piece
    {
        public bool IsMoved { get; set; }

        public Rook(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.Rook)
        {
            IsMoved = false;
        }

        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            // Rook moves horizontally or vertically
            return start.X == end.X || start.Y == end.Y;
        }
    }
}