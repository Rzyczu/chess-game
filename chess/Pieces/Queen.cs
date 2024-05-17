using chess.Enums;

namespace chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Coordinates coordinates, Player player) : base(coordinates, player, PieceType.Queen)
        {
        }

        public override bool IsValidMove(Coordinates start, Coordinates end, GameBoard board)
        {
            int deltaX = Math.Abs(start.X - end.X);
            int deltaY = Math.Abs(start.Y - end.Y);

            // Queen moves horizontally, vertically, or diagonally
            return deltaX == 0 || deltaY == 0 || deltaX == deltaY;
        }
    }
}
