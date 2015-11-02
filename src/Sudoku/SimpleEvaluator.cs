namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SimpleEvaluator : BaseEvaluator {

        public SimpleEvaluator(IMoveFinder moveFinder) {
            if (moveFinder == null) { throw new ArgumentNullException(nameof(moveFinder)); }

            MoveFinder = moveFinder;
        }
        protected IMoveFinder MoveFinder { get; }
        public override IScore GetScore(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            return GetScore(new BoardCells(board));
        }

        //public override IScore GetScore(IBoardCells boardCells) {
        //    if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

        //    // score = (Size*Size - num-non-zero-cells)

        //    if (boardCells.BoardScore == null) {
        //        int score = boardCells.Board.Size * boardCells.Board.Size;
        //        int numNonZero = 0;
        //        for (int row = 0; row < boardCells.Board.Size; row++) {
        //            for (int col = 0; col < boardCells.Board.Size; col++) {
        //                if (boardCells.Board[row, col] != 0) {
        //                    numNonZero++;
        //                }
        //            }
        //        }
        //    }

        //    return boardCells.BoardScore;
        //}

        public override IScore GetScore(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            if (Board.IsSolved((Board)boardCells.Board)) {
                return Score.MaxScore;
            }

            // for this basic implementation the score is 1/(# moves on board)
            var moves = MoveFinder.FindMoves(boardCells);
            int numMoves = moves.Count;
            foreach(var cell in moves) {
                numMoves += cell.Moves.Count;
            }

            if (numMoves == 0) {
                // board should be solved
                if (HasMoves(boardCells.Board)) {
                    // unsolvable board
                    return Score.MinScore;
                }

                // the board is solved
                return Score.MaxScore;
            }

            //int numForcedMoves = 0;
            //foreach(var cellMove in moves) {
            //    foreach(var move in cellMove.Moves) {
            //        if(move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
            //            numForcedMoves++;
            //        }
            //    }
            //}

            // double scorevalue = -1*(2 * (numForcedMoves) + (moves.Count - numForcedMoves));
            //return new Score(scorevalue);
            return new Score(-1.0d * numMoves);
        }

        public override IScore GetScore(IMove move) {
            if(move.MoveScore == null) {
                // move.MoveScore = GetScore(new Board(move.Board, move));

                if (Board.IsSolved((Board)move.Board)) {
                    return Score.MaxScore;
                }

                if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    move.MoveScore = Score.MaxScore;
                }
                else {
                    // score = num of other cells with the same value
                    int score = 0;
                    int numEmptyCells = 0;
                    for(int row = 0; row < move.Board.Size; row++) {
                        for(int col = 0; col < move.Board.Size; col++) {
                            int value = move.Board[row, col];
                            if (value == move.Value) {
                                score++;
                            }
                            else if(value == 0) {
                                numEmptyCells = 0;
                            }
                        }
                    }

                    move.MoveScore = new Score(move.Board.Size * move.Board.Size - numEmptyCells + score);
                }
            }

            return move.MoveScore;
        }
    }
}
