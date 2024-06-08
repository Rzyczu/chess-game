using chess.Components.Pieces;

namespace chess.Components
{
    public class Move
    {
        public Coordinates StartPosition { get; set; }
        public Coordinates EndPosition { get; set; }
        public Piece PiecePlayed { get; set; }
        public Piece? PieceCaptured { get; set; }
        public Piece? Promotion { get; set; }
        public Move(Coordinates startPosition, Coordinates endPosition, Piece piecePlayed, Piece? pieceCaptured)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            PiecePlayed = piecePlayed;
            PieceCaptured = pieceCaptured;
        }

        public void SetPieceCaptured(Piece pieceCaptured)
        {
            PieceCaptured = pieceCaptured;
        }

    }
}
