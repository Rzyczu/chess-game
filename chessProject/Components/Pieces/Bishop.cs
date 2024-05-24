using chess.Components;
using chess.Helpers.Enums;

namespace chess.Components.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.Bishop)
        {
        }

        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            int deltaX = Math.Abs(start.X - end.X);
            int deltaY = Math.Abs(start.Y - end.Y);

            // Bishop moves diagonally, so deltaX should be equal to deltaY
            return deltaX == deltaY;
        }
    }
}
