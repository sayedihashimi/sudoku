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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void WillThrowIfDimensionsDoNotMatch() {
            int[,] data = {
                {0, 1 },
                {0, 3 },
                {0, 2 }
            };

            Assert.Throws<ArgumentException>(() => new Board(data));
        }

        [Fact]
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
    }
}
