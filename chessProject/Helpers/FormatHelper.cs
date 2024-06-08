using chess.Components;
using chess.Components.Pieces;
using chess.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Helpers
{
    public class FormatHelper
    {
        public static Coordinates AlgebraicToCoordinates(string algebraicNotation)
        {
            int row = algebraicNotation[0] - 'a';
            int col = algebraicNotation[1] - '1';
            return new Coordinates(row, col);
        }

        public static string FormatMove(Move move)
        {
            StringBuilder moveNotation = new StringBuilder();

            moveNotation.Append(GetPieceNotation(move.PiecePlayed));
            moveNotation.Append(FormatCoordinates(move.StartPosition));
            moveNotation.Append(move.PieceCaptured != null ? "x" : "-");
            moveNotation.Append(FormatCoordinates(move.EndPosition));
            if (move.Promotion != null)
            {
                moveNotation.Append("=");
                moveNotation.Append(GetPieceNotation(move.Promotion));
            }

            return moveNotation.ToString();
        }

        private static string FormatCoordinates(Coordinates coordinates)
        {
            char file = (char)('a' + coordinates.X);
            char rank = (char)('1' + coordinates.Y);
            return $"{file}{rank}";
        }

        private static string GetPieceNotation(Piece piece)
        {
            return piece.Type switch
            {
                PieceType.King => "K",
                PieceType.Queen => "Q",
                PieceType.Rook => "R",
                PieceType.Bishop => "B",
                PieceType.Knight => "N",
                PieceType.Pawn => "",
                _ => ""
            };
        }

        public static bool IsValidFormatInput(string position)
        {
            return position.Length == 2 &&
                   position[0] >= 'a' && position[0] <= 'h' &&
                   position[1] >= '1' && position[1] <= '8';
        }

        public static bool IsValidInput(string position)
        {
            return position.Length == 2 &&
                   position[0] >= 'a' && position[0] <= 'h' &&
                   position[1] >= '1' && position[1] <= '8';
        }

    }
}
