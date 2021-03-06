﻿namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class MoveFinderTests {
        public class CellTests {
            [Fact]
            public void WillReturnEmptyListCellHasAValue() {
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
                Assert.Equal(0, cellMoves.Moves.Count);

                cellMoves = moveFinder.GetMovesForCell(boardCells, 8, 8);
                Assert.Equal(0, cellMoves.Moves.Count);
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
                Assert.Equal(expectedValues.Length, cellMoves.Moves.Count);

                foreach (IMove move in cellMoves.Moves) {
                    Assert.True(expectedValues.Contains(move.Value));
                }

                cellMoves = moveFinder.GetMovesForCell(boardCells, 8, 7);
                expectedValues = new int[] { 1, 2, 5, 9 };
                // 1,2,5,9
                Assert.Equal(4, cellMoves.Moves.Count);
                foreach (IMove move in cellMoves.Moves) {
                    Assert.True(expectedValues.Contains(move.Value));
                }
            }
        }

        public class BoardTests {
            [Fact]
            public void WillReturnNoMovesForSolvedBoard() {
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

                var finder = new SimpleMoveFinder();
                var moves = finder.FindMoves(new BoardCells(new Board(data)));
                Assert.Equal(0, moves.Count);
            }

            [Fact]
            public void WillReturnOneMoveWhenOneCellIsEmpty() {
                int[,] data = new int[,] {
                    {9,6,2,7,1,5,8,3,4},
                    {4,7,8,9,6,3,5,1,2},
                    {3,1,5,4,2,8,6,9,7},
                    {1,2,7,6,5,9,3,4,8},
                    {8,9,4,2,3,1,7,5,6},
                    {5,3,6,8,7,4,9,2,1},
                    {6,5,3,1,4,7,2,8,9},
                    {7,8,1,3,9,2,4,6,0},
                    {2,4,9,5,8,6,1,7,3}
                };

                var finder = new SimpleMoveFinder();
                var moves = finder.FindMoves(new BoardCells(new Board(data)));
                Assert.Equal(1, moves.Count);
            }

            [Fact]
            public void WillReturnTwoMovesWhenOnlyTwoExist() {
                int[,] data = new int[,] {
                    {9,6,2,7,1,5,8,3,4},
                    {4,7,8,9,6,3,5,1,2},
                    {3,1,5,4,2,8,6,9,7},
                    {1,2,7,6,5,9,3,4,8},
                    {8,9,4,2,3,1,7,5,6},
                    {5,3,6,8,7,4,9,2,1},
                    {6,5,3,1,4,7,2,8,0},
                    {7,8,1,3,9,2,4,6,0},
                    {2,4,9,5,8,6,1,7,3}
                };

                var finder = new SimpleMoveFinder();
                var moves = finder.FindMoves(new BoardCells(new Board(data)));
                Assert.Equal(2, moves.Count);
            }

            [Fact]
            public void TestProblemBoard() {
                IBoard board = new Board("904581600923760580601392470002810900098420100007956320736148259845239716219675843");
                var finder = new SimpleMoveFinder();
                var moves = finder.FindMoves(new BoardCells(board));

                string foo = "bar";

                moves = finder.FindMoves(new BoardCells(new Board("000500000003000000601090400002010900090420100007900320736148259800000710209070003")));
                // now get forced moves
            }
        }
    }
}
