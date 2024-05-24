//using System;
//using Xunit;
//using chess;
//using chess.Pieces;
//using chess.Enums;
//using chess.Helpers;

//namespace chess.Tests
//{
//    public class GameTests
//    {
//        [Fact]
//        public void TestPawnMovement()
//        {
//            Game game = new Game();
//            game.StartGame();

//            Coordinates start = new Coordinates(0, 1); // Initial position of a pawn
//            Coordinates end = new Coordinates(0, 3);   // Move forward two squares

//            game.MakeMove(start, end);

//            Piece piece = game.GetPieceAt(end);
//            Assert.NotNull(piece);
//            Assert.IsType<Pawn>(piece);
//        }

//        [Fact]
//        public void TestKnightMovement()
//        {
//            Game game = new Game();
//            game.StartGame();

//            Coordinates start = new Coordinates(1, 0); // Initial position of a knight
//            Coordinates end = new Coordinates(2, 2);   // Valid knight move

//            game.MakeMove(start, end);

//            Piece piece = game.GetPieceAt(end);
//            Assert.NotNull(piece);
//            Assert.IsType<Knight>(piece);
//        }

//        [Fact]
//        public void TestCapturingPiece()
//        {
//            Game game = new Game();
//            game.StartGame();

//            Coordinates whiteStart = new Coordinates(0, 1);
//            Coordinates whiteEnd = new Coordinates(0, 3);
//            game.MakeMove(whiteStart, whiteEnd); // Move white pawn

//            Coordinates blackStart = new Coordinates(0, 6);
//            Coordinates blackEnd = new Coordinates(0, 4);
//            game.MakeMove(blackStart, blackEnd); // Move black pawn

//            whiteEnd = new Coordinates(0, 4);
//            game.MakeMove(new Coordinates(0, 3), whiteEnd); // White pawn captures black pawn

//            Piece piece = game.GetPieceAt(whiteEnd);
//            Assert.NotNull(piece);
//            Assert.IsType<Pawn>(piece);
//            Assert.Equal(ColorType.White, piece.Player.Color);
//        }

//        [Fact]
//        public void TestKingSideCastling()
//        {
//            Game game = new Game();
//            game.StartGame();

//            // Move pawns to clear path for castling
//            game.MakeMove(new Coordinates(4, 1), new Coordinates(4, 3));
//            game.MakeMove(new Coordinates(5, 0), new Coordinates(2, 3));
//            game.MakeMove(new Coordinates(6, 0), new Coordinates(5, 2));

//            // Perform castling
//            Coordinates kingStart = new Coordinates(4, 0);
//            Coordinates kingEnd = new Coordinates(6, 0);
//            game.MakeMove(kingStart, kingEnd);

//            Piece king = game.GetPieceAt(kingEnd);
//            Assert.NotNull(king);
//            Assert.IsType<King>(king);

//            Coordinates rookEnd = new Coordinates(5, 0);
//            Piece rook = game.GetPieceAt(rookEnd);
//            Assert.NotNull(rook);
//            Assert.IsType<Rook>(rook);
//        }

//        [Fact]
//        public void TestPuttingKingInCheck()
//        {
//            Game game = new Game();
//            game.StartGame();

//            // Move pieces to set up check
//            game.MakeMove(new Coordinates(4, 1), new Coordinates(4, 3));
//            game.MakeMove(new Coordinates(5, 0), new Coordinates(2, 3));
//            game.MakeMove(new Coordinates(3, 6), new Coordinates(3, 4));
//            game.MakeMove(new Coordinates(2, 3), new Coordinates(6, 7));

//            King blackKing = (King)game.GetPieceOfType(PieceType.King, game.GetPlayer(ColorType.Black));
//            Assert.True(blackKing.IsInCheck);
//        }

//        [Fact]
//        public void TestPuttingKingInCheckmate()
//        {
//            Game game = new Game();
//            game.StartGame();

//            // Move pieces to set up checkmate
//            game.MakeMove(new Coordinates(5, 1), new Coordinates(5, 3)); // White pawn to e4
//            game.MakeMove(new Coordinates(4, 6), new Coordinates(4, 4)); // Black pawn to e5
//            game.MakeMove(new Coordinates(3, 0), new Coordinates(7, 4)); // White queen to h5
//            game.MakeMove(new Coordinates(1, 7), new Coordinates(2, 5)); // Black knight to c6
//            game.MakeMove(new Coordinates(7, 4), new Coordinates(5, 6)); // White queen to f7 (checkmate)

//            King blackKing = (King)game.board.GetPieceOfType(PieceType.King, game.GetPlayer(ColorType.Black));
//            Assert.True(blackKing.IsInCheck);
//            Assert.False(game.CanEscapeCheck(game.GetPlayer(ColorType.Black)));
//        }
//    }
//}
