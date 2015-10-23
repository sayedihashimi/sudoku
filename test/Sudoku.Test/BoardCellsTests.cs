namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BoardCellsTests {
        [Fact]
        public void WillReturnRows() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var rows = boardCells.Rows;
            Assert.Equal(9, rows.Count);
            Assert.True(rows[0].SequenceEqual(new int[] { 9, 0, 0, 0, 0, 5, 6, 8, 1 }));
            Assert.True(rows[1].SequenceEqual(new int[] { 0, 6, 0, 2, 8, 0, 7, 0, 0 }));
            Assert.True(rows[2].SequenceEqual(new int[] { 0, 0, 0, 0, 0, 6, 9, 0, 5 }));
            Assert.True(rows[3].SequenceEqual(new int[] { 0, 8, 0, 0, 0, 2, 0, 4, 6 }));
            Assert.True(rows[4].SequenceEqual(new int[] { 0, 0, 5, 0, 0, 0, 3, 0, 0 }));
            Assert.True(rows[5].SequenceEqual(new int[] { 1, 9, 0, 5, 0, 0, 0, 7, 0 }));
            Assert.True(rows[6].SequenceEqual(new int[] { 8, 0, 2, 9, 0, 0, 0, 0, 0 }));
            Assert.True(rows[7].SequenceEqual(new int[] { 0, 0, 9, 0, 2, 7, 0, 6, 0 }));
            Assert.True(rows[8].SequenceEqual(new int[] { 6, 7, 4, 8, 0, 0, 0, 0, 3 }));
        }

        [Fact]
        public void WillReturnColumns() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var columns = boardCells.Columns;
            Assert.Equal(9, columns.Count);
            Assert.True(columns[0].SequenceEqual(new int[] { 9, 0, 0, 0, 0, 1, 8, 0, 6 }));
            Assert.True(columns[1].SequenceEqual(new int[] { 0, 6, 0, 8, 0, 9, 0, 0, 7 }));
            Assert.True(columns[2].SequenceEqual(new int[] { 0, 0, 0, 0, 5, 0, 2, 9, 4 }));
            Assert.True(columns[3].SequenceEqual(new int[] { 0, 2, 0, 0, 0, 5, 9, 0, 8 }));
            Assert.True(columns[4].SequenceEqual(new int[] { 0, 8, 0, 0, 0, 0, 0, 2, 0 }));
            Assert.True(columns[5].SequenceEqual(new int[] { 5, 0, 6, 2, 0, 0, 0, 7, 0 }));
            Assert.True(columns[6].SequenceEqual(new int[] { 6, 7, 9, 0, 3, 0, 0, 0, 0 }));
            Assert.True(columns[7].SequenceEqual(new int[] { 8, 0, 0, 4, 0, 7, 0, 6, 0 }));
            Assert.True(columns[8].SequenceEqual(new int[] { 1, 0, 5, 6, 0, 0, 0, 0, 3 }));
        }

        [Fact]
        public void WillReturnSquares() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var squares = boardCells.Squares;
            Assert.Equal(9, squares.Count);

            Assert.Equal(9, squares.ElementAt(0)[0, 0]);
            Assert.Equal(0, squares.ElementAt(0)[0, 1]);
            Assert.Equal(0, squares.ElementAt(0)[0, 2]);
            Assert.Equal(0, squares.ElementAt(0)[1, 0]);
            Assert.Equal(6, squares.ElementAt(0)[1, 1]);
            Assert.Equal(0, squares.ElementAt(0)[1, 2]);
            Assert.Equal(0, squares.ElementAt(0)[2, 0]);
            Assert.Equal(0, squares.ElementAt(0)[2, 1]);
            Assert.Equal(0, squares.ElementAt(0)[2, 2]);

            Assert.Equal(0, squares.ElementAt(1)[0, 0]);
            Assert.Equal(0, squares.ElementAt(1)[0, 1]);
            Assert.Equal(5, squares.ElementAt(1)[0, 2]);
            Assert.Equal(2, squares.ElementAt(1)[1, 0]);
            Assert.Equal(8, squares.ElementAt(1)[1, 1]);
            Assert.Equal(0, squares.ElementAt(1)[1, 2]);
            Assert.Equal(0, squares.ElementAt(1)[2, 0]);
            Assert.Equal(0, squares.ElementAt(1)[2, 1]);
            Assert.Equal(6, squares.ElementAt(1)[2, 2]);

            Assert.Equal(6, squares.ElementAt(2)[0, 0]);
            Assert.Equal(8, squares.ElementAt(2)[0, 1]);
            Assert.Equal(1, squares.ElementAt(2)[0, 2]);
            Assert.Equal(7, squares.ElementAt(2)[1, 0]);
            Assert.Equal(0, squares.ElementAt(2)[1, 1]);
            Assert.Equal(0, squares.ElementAt(2)[1, 2]);
            Assert.Equal(9, squares.ElementAt(2)[2, 0]);
            Assert.Equal(0, squares.ElementAt(2)[2, 1]);
            Assert.Equal(5, squares.ElementAt(2)[2, 2]);

            Assert.Equal(0, squares.ElementAt(3)[0, 0]);
            Assert.Equal(8, squares.ElementAt(3)[0, 1]);
            Assert.Equal(0, squares.ElementAt(3)[0, 2]);
            Assert.Equal(0, squares.ElementAt(3)[1, 0]);
            Assert.Equal(0, squares.ElementAt(3)[1, 1]);
            Assert.Equal(5, squares.ElementAt(3)[1, 2]);
            Assert.Equal(1, squares.ElementAt(3)[2, 0]);
            Assert.Equal(9, squares.ElementAt(3)[2, 1]);
            Assert.Equal(0, squares.ElementAt(3)[2, 2]);

            Assert.Equal(0, squares.ElementAt(4)[0, 0]);
            Assert.Equal(0, squares.ElementAt(4)[0, 1]);
            Assert.Equal(2, squares.ElementAt(4)[0, 2]);
            Assert.Equal(0, squares.ElementAt(4)[1, 0]);
            Assert.Equal(0, squares.ElementAt(4)[1, 1]);
            Assert.Equal(0, squares.ElementAt(4)[1, 2]);
            Assert.Equal(5, squares.ElementAt(4)[2, 0]);
            Assert.Equal(0, squares.ElementAt(4)[2, 1]);
            Assert.Equal(0, squares.ElementAt(4)[2, 2]);

            Assert.Equal(0, squares.ElementAt(5)[0, 0]);
            Assert.Equal(4, squares.ElementAt(5)[0, 1]);
            Assert.Equal(6, squares.ElementAt(5)[0, 2]);
            Assert.Equal(3, squares.ElementAt(5)[1, 0]);
            Assert.Equal(0, squares.ElementAt(5)[1, 1]);
            Assert.Equal(0, squares.ElementAt(5)[1, 2]);
            Assert.Equal(0, squares.ElementAt(5)[2, 0]);
            Assert.Equal(7, squares.ElementAt(5)[2, 1]);
            Assert.Equal(0, squares.ElementAt(5)[2, 2]);

            Assert.Equal(8, squares.ElementAt(6)[0, 0]);
            Assert.Equal(0, squares.ElementAt(6)[0, 1]);
            Assert.Equal(2, squares.ElementAt(6)[0, 2]);
            Assert.Equal(0, squares.ElementAt(6)[1, 0]);
            Assert.Equal(0, squares.ElementAt(6)[1, 1]);
            Assert.Equal(9, squares.ElementAt(6)[1, 2]);
            Assert.Equal(6, squares.ElementAt(6)[2, 0]);
            Assert.Equal(7, squares.ElementAt(6)[2, 1]);
            Assert.Equal(4, squares.ElementAt(6)[2, 2]);

            Assert.Equal(9, squares.ElementAt(7)[0, 0]);
            Assert.Equal(0, squares.ElementAt(7)[0, 1]);
            Assert.Equal(0, squares.ElementAt(7)[0, 2]);
            Assert.Equal(0, squares.ElementAt(7)[1, 0]);
            Assert.Equal(2, squares.ElementAt(7)[1, 1]);
            Assert.Equal(7, squares.ElementAt(7)[1, 2]);
            Assert.Equal(8, squares.ElementAt(7)[2, 0]);
            Assert.Equal(0, squares.ElementAt(7)[2, 1]);
            Assert.Equal(0, squares.ElementAt(7)[2, 2]);

            Assert.Equal(0, squares.ElementAt(8)[0, 0]);
            Assert.Equal(0, squares.ElementAt(8)[0, 1]);
            Assert.Equal(0, squares.ElementAt(8)[0, 2]);
            Assert.Equal(0, squares.ElementAt(8)[1, 0]);
            Assert.Equal(6, squares.ElementAt(8)[1, 1]);
            Assert.Equal(0, squares.ElementAt(8)[1, 2]);
            Assert.Equal(0, squares.ElementAt(8)[2, 0]);
            Assert.Equal(0, squares.ElementAt(8)[2, 1]);
            Assert.Equal(3, squares.ElementAt(8)[2, 2]);
        }

        [Fact]
        public void CanGetRowForCell() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var row1 = boardCells.GetRowForCell(0, 0);
            var row2 = boardCells.GetRowForCell(8, 8);
            var row3 = boardCells.GetRowForCell(3, 5);

            Assert.True(row1.SequenceEqual(new int[] { 9, 0, 0, 0, 0, 5, 6, 8, 1 }));
            Assert.True(row2.SequenceEqual(new int[] { 6, 7, 4, 8, 0, 0, 0, 0, 3 }));
            Assert.True(row3.SequenceEqual(new int[] { 0, 8, 0, 0, 0, 2, 0, 4, 6 }));
        }

        public void CanGetColsForCell() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var col1 = boardCells.GetColumnForCell(0, 0);
            var col2 = boardCells.GetColumnForCell(8, 8);
            var col3 = boardCells.GetColumnForCell(3, 5);

            Assert.True(col1.SequenceEqual(new int[] { 9, 0, 0, 0, 0, 1, 8, 0, 6  }));
            Assert.True(col2.SequenceEqual(new int[] { 1, 0, 5, 6, 0, 0, 0, 0, 3 }));
            Assert.True(col3.SequenceEqual(new int[] { 0, 2, 0, 0, 0, 5, 9, 0, 8 }));
        }

        public void CanGetSquareForCell() {
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

            IBoardCells boardCells = new BoardCells(new Board(data));
            var sq1 = boardCells.GetSquareForCell(0, 0);
            var sq2 = boardCells.GetSquareForCell(8, 8);
            var sq3 = boardCells.GetSquareForCell(3, 5);

            Assert.Equal(9, sq1[0, 0]);
            Assert.Equal(0, sq1[0, 1]);
            Assert.Equal(0, sq1[0, 2]);
            Assert.Equal(0, sq1[1, 0]);
            Assert.Equal(6, sq1[1, 1]);
            Assert.Equal(0, sq1[1, 2]);
            Assert.Equal(0, sq1[2, 0]);
            Assert.Equal(0, sq1[2, 1]);
            Assert.Equal(0, sq1[2, 2]);

            Assert.Equal(0, sq2[0, 0]);
            Assert.Equal(0, sq2[0, 1]);
            Assert.Equal(0, sq2[0, 2]);
            Assert.Equal(0, sq2[1, 0]);
            Assert.Equal(6, sq2[1, 1]);
            Assert.Equal(0, sq2[1, 2]);
            Assert.Equal(0, sq2[2, 0]);
            Assert.Equal(0, sq2[2, 1]);
            Assert.Equal(3, sq2[2, 2]);

            Assert.Equal(0, sq3[0, 0]);
            Assert.Equal(0, sq3[0, 1]);
            Assert.Equal(2, sq3[0, 2]);
            Assert.Equal(0, sq3[1, 0]);
            Assert.Equal(0, sq3[1, 1]);
            Assert.Equal(0, sq3[1, 2]);
            Assert.Equal(5, sq3[2, 0]);
            Assert.Equal(0, sq3[2, 1]);
            Assert.Equal(0, sq3[2, 2]);
        }
    }
}
