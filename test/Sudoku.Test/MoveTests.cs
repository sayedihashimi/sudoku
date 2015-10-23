namespace Sudoku.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class MoveTests {
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

            IMove move1 = new Move(board1, 0, 1, 2);
            IMove move2 = new Move(board1, 0, 1, 2);
            IMove move3 = new Move(board2, 0, 1, 2);

            Assert.True(move1.Equals(move2));
            Assert.True(move1.Equals(move3));
            Assert.True(move2.Equals(move1));
            Assert.True(move2.Equals(move3));
            Assert.True(move3.Equals(move1));
            Assert.True(move3.Equals(move2));
        }
        [Fact]
        public void EqualsReturnsFalseWhenValueIsDifferent() {
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

            IBoard board1 = new Board(data1);

            IMove move1 = new Move(board1, 0, 1, 2);
            IMove move2 = new Move(board1, 0, 1, 3);

            Assert.False(move1.Equals(move2));
            Assert.False(move2.Equals(move1));
        }

        [Fact]
        public void EqualsReturnsFalseWhenRowIsDifferent() {
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

            IBoard board1 = new Board(data1);

            IMove move1 = new Move(board1, 0, 1, 2);
            IMove move2 = new Move(board1, 1, 1, 2);

            Assert.False(move1.Equals(move2));
            Assert.False(move2.Equals(move1));
        }

        [Fact]
        public void EqualsReturnsFalseWhenColumnIsDifferent() {
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

            IBoard board1 = new Board(data1);

            IMove move1 = new Move(board1, 0, 1, 2);
            IMove move2 = new Move(board1, 0, 2, 2);

            Assert.False(move1.Equals(move2));
            Assert.False(move2.Equals(move1));
        }

        [Fact]
        public void EqualsReturnsFalseWhenBoardIsDifferent() {
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
                {6,7,4, 8,0,0, 0,1,3 }
            };

            IBoard board1 = new Board(data1);
            IBoard board2 = new Board(data2);

            IMove move1 = new Move(board1, 0, 1, 2);
            IMove move2 = new Move(board2, 0, 1, 2);

            Assert.False(move1.Equals(move2));
            Assert.False(move2.Equals(move1));
        }
    }
}
