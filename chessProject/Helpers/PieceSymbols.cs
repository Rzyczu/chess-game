using chess.Enums;

public static class PieceSymbols
{
    public static readonly Dictionary<PieceType, char> Symbols = new Dictionary<PieceType, char>
    {
        { PieceType.King, '\u2654' },   // White King ♔
        { PieceType.Queen, '\u2655' },  // White Queen ♕
        { PieceType.Rook, '\u2656' },   // White Rook ♖
        { PieceType.Bishop, '\u2657' }, // White Bishop ♗
        { PieceType.Knight, '\u2658' }, // White Knight ♘
        { PieceType.Pawn, '\u2659' }    // White Pawn ♙
    };
}