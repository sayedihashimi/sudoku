namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    // test site: http://www.sudoku-solutions.com/
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

        [Fact]
        public void CanSolveAMediumBoard01() {
            IBoard board = new Board(".5..8..3.16...5.74...9...6..4..7.1.2.........5.1.6..4..7...8.1.41.7...23.3..1..9.");
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(board);

            IBoard solvedBoard = new Board("954687231168325974327941568643879152792154386581263749279438615415796823836512497");
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        }

        [Fact]
        public void CanSolveAHardBoard01() {
            IBoard board = new Board("...........3......6.1.9.4....2.1.9...9.42.1....79..32..36.48..98.....71.2.9.7...3");
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(board);

            IBoard solvedBoard = new Board("475381692923764581681592437562813974398427165147956328736148259854239716219675843");
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        }
        
        // [Fact]
        public void CanSolveAHardBoard02() {
            IBoard board = new Board("..15.....4......7..6..9.1..3....15...9..6...8..57...4..8.1....2..3....5......26..");
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(board);

            IBoard solvedBoard = new Board("931576824452813976867294135328941567794365218615728349586137492243689751179452683");
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        }

        [Fact]
        public void CanSolveADiabolicalBoard01() {
            IBoard board = new Board("...7.4..5.2..1..7.....8...2.9...625.6...7...8.532...1.4...9.....3..6..9.2..4.7...");
            var moveFinder = new SimpleMoveFinder();
            var player = new SimplePlayer(moveFinder, new SimpleEvaluator(moveFinder));
            var result = player.SolveBoard(board);

            IBoard solvedBoard = new Board("981724365324615879765983142197836254642571938853249716476398521538162497219457683");
            Assert.Equal(solvedBoard, result.CurrentBoard.Board);
        }

    }
}
