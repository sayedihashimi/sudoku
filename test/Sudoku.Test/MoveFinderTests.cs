namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class MoveFinderTests {
        [Fact]
        public void WillReturnNullIfCellHasAValue() {
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
            SimpleMoveFinder moveFinder = new SimpleMoveFinder();

            var cellMoves = moveFinder.GetMovesForCell(boardCells, 0, 0);
            Assert.Null(cellMoves);

            cellMoves = moveFinder.GetMovesForCell(boardCells, 8, 8);
            Assert.Null(cellMoves);
        }

        [Fact]
        public void WilLReturnMoves() {
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
            SimpleMoveFinder moveFinder = new SimpleMoveFinder();

            var cellMoves = moveFinder.GetMovesForCell(boardCells, 0, 1);
            // 2,3,4
            int[] expectedValues = new int[] { 2, 3, 4 };
            Assert.Equal(expectedValues.Length, cellMoves.Count);

            foreach(IMove move in cellMoves) {
                Assert.True(expectedValues.Contains(move.Value));
            }

            cellMoves = moveFinder.GetMovesForCell(boardCells, 8, 7);
            expectedValues = new int[] { 1, 2, 5, 9 };
            // 1,2,5,9
            Assert.Equal(4, cellMoves.Count);
            foreach (IMove move in cellMoves) {
                Assert.True(expectedValues.Contains(move.Value));
            }
        }
    }
}
