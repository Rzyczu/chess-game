using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class ErrorMessages
    {
        public static readonly string MoveFormatInputError = "\x1b[31mInvalid move format. \x1b[0m";
        public static readonly string WithinBoundError = "\x1b[31mPosition is out of bounds. \x1b[0m";
        public static readonly string PieceStartError = "\x1b[31mNo piece at start position. \x1b[0m";
        public static readonly string PieceEndError = "\x1b[31mNo piece at end position. \x1b[0m";
        public static readonly string InvalidMoveError = "\x1b[31mInvalid move. Please try again. \x1b[0m";
        public static readonly string CatlingPieceMovedError = "\x1b[31mInvalid castling. King or rook has already moved. \x1b[0m";
        public static readonly string CastlingPacthError = "\x1b[31mInvalid castling. Pieces obstruct the path. \x1b[0m";
        
        public static string EnemyPieceStartError(Player player) => $"\x1b[31mStart position doesn't contain {player.Color} piece. \x1b[0m";

    }
}
