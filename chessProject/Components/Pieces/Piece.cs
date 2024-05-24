using chess.Components;
using chess.Helpers.Enums;

namespace chess.Components.Pieces
{
    public abstract class Piece
    {
        public Coordinates Coordinates { get; set; }
        public Player Player { get; set; }
        public PieceType Type { get; set; }

        public Piece(Coordinates coordinates, Player player, PieceType type)
        {
            Coordinates = coordinates;
            Player = player;
            Type = type;
        }

        // Validate whether the move is legal for this piece
        public abstract bool IsValidMove(Coordinates start, Coordinates end, GameBoard board);
    }
}
