namespace chess
{
    public static class ErrorMessages
    {
        public static readonly string MoveFormatInputError = "Invalid move format.";
        public static readonly string WithinBoundError = "Position is out of bounds.";
        public static readonly string PieceStartError = "No piece at start position.";
        public static readonly string InvalidMoveError = "Invalid move. Please try again.";
        public static readonly string InvalidPacthError = "Pieces obstruct the path.";
        public static readonly string NoKingInCheckMoveError = "King is in check.";
        public static readonly string CatlingPieceMovedError = "Invalid castling. King or rook has already moved.";
        public static readonly string CastlingPacthError = "Invalid castling. Pieces obstruct the path.";

        public static string EnemyPieceStartError(Turn currentTurn) => $"Start position doesn't contain {currentTurn.Player.Color} piece.";
    }

    public static class InfoMessages
    {
        public static readonly string GameOverInfo = "Game over!";
        public static readonly string EnterMoveInfo = "Enter your move (e.g., 'e2 e4'): ";

        public static string CurrentTurnInfo(Turn currentTurn) => $"Turn: {currentTurn.Number}.";
        public static string CurrentPlayerInfo(Turn currentTurn) => $"Player {currentTurn.Player.Color} move.";

    }

    public static class WarningMessages
    {
        public static string KingInCheckWarning(Turn currentTurn) => $"{currentTurn.Player.Color} King is in check.";

    }

}
