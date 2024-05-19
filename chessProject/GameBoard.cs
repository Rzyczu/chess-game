﻿using chess.Enums;
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

        public GameBoard(int width, int height)
        {
            Height = height;
            Width = width;
            BoardArray = new Piece[width, height];
        }


        public void AddPiece(Piece piece, Coordinates position)
        {
            if (IsWithinBounds(position))
            {
                piece.Coordinates = position;
                BoardArray[position.X, position.Y] = piece; // Updated indexing
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

        public Piece? GetPieceAt(Coordinates position)
        {
            if (IsWithinBounds(position))
                return BoardArray[position.X, position.Y]; // Updated indexing
            else
                return null;
        }

        public void RemovePieceAt(Coordinates position)
        {
            if (IsWithinBounds(position))
            {
                Piece pieceToRemove = BoardArray[position.Y, position.X]; // Updated indexing
                if (pieceToRemove != null)
                {
                    if (pieceToRemove.Player.Color == ColorType.White)
                        WhitePieces.Remove(pieceToRemove);
                    else
                        BlackPieces.Remove(pieceToRemove);
                    BoardArray[position.X, position.Y] = null; // Updated indexing
                }
            }
            else
            {
                throw new ArgumentException("Position is out of bounds.");
            }
        }

        public Piece GetPieceOfType(PieceType type, Player player)
        {
            List<Piece> pieces = (player.Color == ColorType.White) ? WhitePieces : BlackPieces;
            return pieces.FirstOrDefault(piece => piece.Type == type);
        }

        public bool IsWithinBounds(Coordinates position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height; // Updated conditions
        }
    }
}
