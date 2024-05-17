using chess.Enums;
using chess.Pieces;
using System;
using System.Collections.Generic;

namespace chess
{
    public class GameBoard
    {
        private readonly Piece[,] BoardArray;
        public int Height { get; }
        public int Width { get; }
        public List<Piece> WhitePieces { get; } = new List<Piece>();
        public List<Piece> BlackPieces { get; } = new List<Piece>();

        public GameBoard(int height, int width)
        {
            Height = height;
            Width = width;
            BoardArray = new Piece[height, width];
        }


        public void AddPiece(Piece piece, Coordinates position)
        {
            if (IsWithinBounds(position))
            {
                piece.Coordinates = position;
                BoardArray[position.X, position.Y] = piece;
                if (piece.Player.Color == ColorType.White)
                    WhitePieces.Add(piece);
                else
                    BlackPieces.Add(piece);
            }
            else
            {
                throw new ArgumentException("Position is out of bounds.");
            }
        }

        public Piece GetPieceAt(Coordinates position)
        {
            if (IsWithinBounds(position))
                return BoardArray[position.X, position.Y];
            else
                throw new ArgumentException("Position is out of bounds.");
        }

        public void RemovePieceAt(Coordinates position)
        {
            if (IsWithinBounds(position))
            {
                Piece pieceToRemove = BoardArray[position.X, position.Y];
                if (pieceToRemove != null)
                {
                    if (pieceToRemove.Player.Color == ColorType.White)
                        WhitePieces.Remove(pieceToRemove);
                    else
                        BlackPieces.Remove(pieceToRemove);
                    BoardArray[position.X, position.Y] = null;
                }
            }
            else
            {
                throw new ArgumentException("Position is out of bounds.");
            }
        }

        public bool IsWithinBounds(Coordinates position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
        }
    }
}