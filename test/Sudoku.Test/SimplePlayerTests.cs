namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class SimplePlayerTests {
        [Fact]
        public void WillReturnEmptyListWhenTheBoardIsAlreadySolved() {
            int[,] data = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,3}
            };
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(new Board(data));

            int[,] solveddata = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,3}
            };
            IBoard solvedBoard = new Board(solveddata);
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);

            Assert.Equal(0, result.MovesPlayed.Count);
        }

        [Fact]
        public void CanSolveABoardWithOneMoveRemaining() {
            int[,] data = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,0}
            };
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(new Board(data));

            int[,] solveddata = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,3}
            };
            IBoard solvedBoard = new Board(solveddata);
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);

            Assert.Equal(1, result.MovesPlayed.Count);
            Assert.Equal(3, result.MovesPlayed[0].Value);
            Assert.Equal(8, result.MovesPlayed[0].Row);
            Assert.Equal(8, result.MovesPlayed[0].Column);
        }

        [Fact]
        public void CanSolveABoardWithTwoForcedMovesRemaining() {
            int[,] data = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,0,5},
                {2,4,9,5,8,6,1,7,0}
            };
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(new Board(data));

            Assert.Equal(2, result.MovesPlayed.Count);

            var mv1 = from m in result.MovesPlayed
                      where m.Row == 8 && m.Column == 8 && m.Value == 3
                      select m;

            var mv2 = from m in result.MovesPlayed
                      where m.Row == 7 && m.Column == 7 && m.Value == 6
                      select m;

            int[,] solveddata = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,3}
            };
            IBoard solvedBoard = new Board(solveddata);
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);

            Assert.NotNull(mv1);
            Assert.NotNull(mv2);
        }

        [Fact]
        public void CanSolveASimpleBoard() {
            IBoard board = new Board("8..3......9.....7...3584...4...9...2...4.7...78...1..5..96...2.....5..31........6");
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(board);

            IBoard solvedBoard = new Board("812379654594126378673584219431895762925467183786231945159643827267958431348712596");
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        }

        //[Fact]
        //public void CanSolveAHardBoard() {
        //    IBoard board = new Board("92....3...6.31...4...6..1...7..3...2.4..8..3.1...2..7...9..7...3...48.9...4....58");
        //    var moveFinder = new SimpleMoveFinder();
        //    var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
        //    var result = player.SolveBoard(board);

        //    IBoard solvedBoard = new Board("921874365865319724437652189678931542542786931193425876289567413356148297714293658");
        //    Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        //}
    }
}
