namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BoardTests {

        [Fact]
        public void TestConstructor_ValidData() {
            int[,] data = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 0,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };

            IBoard board = new Board(data);
            Assert.Equal(9, board.Size);

            Assert.Equal(9, board[0, 0]);
            Assert.Equal(6, board[1, 1]);
            Assert.Equal(3, board[4, 6]);

            for(int row = 0; row < 9; row++) {
                for(int col = 0; col < 9; col++) {
                    Assert.Equal(data[row, col], board[row, col]);
                }
            }
        }

        // [Fact]
        public void WillThrowIfHasANegativeValue() {
            int[,] data = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, -1,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };

            Assert.Throws<ArgumentException>(() => new Board(data));
        }

        // [Fact]
        public void WillThrowIfHasNumberExceedingMax() {
            int[,] data = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 1,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 12,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };

            Assert.Throws<ArgumentException>(() => new Board(data));
        }

        // [Fact]
        public void WillThrowIfDimensionsDoNotMatch() {
            int[,] data = {
                {0, 1 },
                {0, 3 },
                {0, 2 }
            };

            Assert.Throws<ArgumentException>(() => new Board(data));
        }

        // [Fact]
        public void WillThrowIfDimensionsAreNotAbleToBeSquared() {
            int[,] data = {
                {0,1,2,3,2 },
                {1,8,2,3,4 },
                {0,1,7,3,5 },
                {2,1,2,6,4 },
                {0,1,2,3,5 },
            };

            Assert.Throws<ArgumentException>(() => new Board(data));
        }

        [Fact]
        public void EqualsReturnsTrueWhenEqual() {
            int[,] data1 = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 0,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };

            int[,] data2 = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 0,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };


            IBoard board1 = new Board(data1);
            IBoard board2 = new Board(data2);

            Assert.True(board1.Equals(board2));
            Assert.True(board2.Equals(board1));
            Assert.True(board1.Equals(board1));
            Assert.True(board2.Equals(board2));
        }

        [Fact]
        public void EqualsReturnsFalseWhenNotEqual() {
            int[,] data1 = {
                {9,0,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 0,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };

            int[,] data2 = {
                {9,1,0, 0,0,5, 6,8,1 },
                {0,6,0, 2,8,0, 7,0,0 },
                {0,0,0, 0,0,6, 9,0,5 },
                {0,8,0, 0,0,2, 0,4,6 },
                {0,0,5, 0,0,0, 3,0,0 },
                {1,9,0, 5,0,0, 0,7,0 },
                {8,0,2, 9,0,0, 0,0,0 },
                {0,0,9, 0,2,7, 0,6,0 },
                {6,7,4, 8,0,0, 0,0,3 }
            };


            IBoard board1 = new Board(data1);
            IBoard board2 = new Board(data2);

            Assert.False(board1.Equals(board2));
            Assert.False(board2.Equals(board1));
        }

        [Fact]
        public void WillDetectDuplicateInRow() {
            Board board = new Board("8.......7..27.83...7..9..4..4..7..9...86.51...15.3..5..8..4..2...35.94..5.......1");
            Assert.False(Board.IsValid(board));
        }

        [Fact]
        public void WillDetectDuplicateInCol() {
            Board board = new Board("..87..4.1..2..5...4.5.817......78......3....56.....9....3..2.4....5..3..2..4.8..6");
            Assert.False(Board.IsValid(board));
        }

        [Fact]
        public void WillDetectDuplicateInSquare() {
            Board board = new Board("..7.....8.6..8..3.5..6..4....9..1....3.....1....4..2....3..8..1.8..5..6.4..8..5..");
            Assert.False(Board.IsValid(board));
        }
    }
}
