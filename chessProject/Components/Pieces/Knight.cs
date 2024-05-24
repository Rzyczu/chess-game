using chess.Components;
using chess.Helpers.Enums;

namespace chess.Components.Pieces
{
    public class Knight : Piece
    {
        public Knight(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.Knight)
        {
        }

        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            int deltaX = Math.Abs(start.X - end.X);
            int deltaY = Math.Abs(start.Y - end.Y);

            // Knight moves in an L-shape (2 squares in one direction and 1 in another)
            return deltaX == 1 && deltaY == 2 || deltaX == 2 && deltaY == 1;
        }
    }
}
