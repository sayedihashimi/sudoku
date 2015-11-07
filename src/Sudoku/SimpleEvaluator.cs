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

        public override IScore GetScore(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            if (Board.IsSolved((Board)boardCells.Board)) {
                return Score.SolvedBoardScore;
            }

            if (boardCells.BoardScore == null) {
                // for this basic implementation the score is 1/(# moves on board)
                var moves = MoveFinder.FindMoves(boardCells);
                int numMoves = 0;
                int numForcedMoves = 0;
                foreach (var cell in moves) {
                    numMoves += cell.Moves.Count;

                    foreach (var move in cell.Moves) {
                        if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                            numForcedMoves++;
                        }
                    }
                }

                if (numMoves == 0) {
                    // board should be solved
                    if (HasMoves(boardCells.Board)) {
                        // unsolvable board
                        return Score.InvalidBoardScore;
                    }

                    // the board is solved
                    return Score.SolvedBoardScore;
                }

                double score = -1 * numMoves;
                if (numForcedMoves > 0) {
                    score += ((double)numForcedMoves / (double)numMoves) * 0.5;
                }
                boardCells.BoardScore = new MultiPartScore(new double[1] { score });
                // return new Score(score);
                // return new Score(1.0d/(1+numMoves + (numMoves-numForcedMoves)));
            }
            else {
                var foo = "bar";
            }

            return boardCells.BoardScore;
        }

        public override IScore GetScore(IMove move) {
            if (move.MoveScore == null) {
                if (Board.IsSolved((Board)move.Board)) {
                    return Score.SolvedBoardScore;
                }

                if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    move.MoveScore = Score.ForcedMoveScore;
                }
                else {
                    // score = num of other cells with the same value
                    int score = 0;
                    int numEmptyCells = 0;
                    for (int row = 0; row < move.Board.Size; row++) {
                        for (int col = 0; col < move.Board.Size; col++) {
                            int value = move.Board[row, col];
                            if (value == move.Value) {
                                score++;
                            }
                            else if (value == 0) {
                                numEmptyCells++;
                            }
                        }
                    }

                    // find sibling cells
                    int numNonEmptySiblings = 0;
                    for (int col = 0; col < move.Board.Size; col++) {
                        if (move.Board[move.Row, col] != 0) {
                            numNonEmptySiblings++;
                        }
                    }
                    for (int row = 0; row < move.Board.Size; row++) {
                        if (move.Board[row, move.Column] != 0) {
                            numNonEmptySiblings++;
                        }
                    }

                    int sqSize = (int)Math.Sqrt(move.Board.Size);
                    int sqRowIndex = (int)(Math.Floor((double)(move.Row / sqSize)));
                    int sqColIndex = (int)(Math.Floor((double)(move.Column / sqSize)));
                    int startRow = sqRowIndex * sqSize;
                    int startCol = sqColIndex * sqSize;
                    for (int r = sqRowIndex; r < sqRowIndex + sqSize; r++) {
                        for (int c = startCol; c < startCol + sqSize; c++) {
                            if (move.Board[r, c] != 0) {
                                numNonEmptySiblings++;
                            }
                        }
                    }

                    move.MoveScore = new MultiPartScore(new double[] { numEmptyCells, score, numNonEmptySiblings });
                }
            }

            return move.MoveScore;
        }

        public override IScore GetCellScore(Cell cell) {
            if (cell.CellScore == null) {
                IScore score = Score.EmptyScore;

                if (cell.Moves.Count > 0) {
                    foreach (var move in cell.Moves) {
                        if (move.MoveScore == null) {
                            move.MoveScore = GetScore(move);
                            score = (MultiPartScore)score + (MultiPartScore)move.MoveScore;
                        }
                    }
                }
                cell.CellScore = score;
            }

            return cell.CellScore;
        }

        public override IScore GetRowScore(IBoardCells board, int row) {
            // row score = #-1*empty cells.#total moves in row gd
            int numMoves = 0;
            int numEmptyCells = 0;
            for (int col = 0; col < board.Board.Size; col++) {
                var cell = MoveFinder.GetMovesForCell(board, row, col);

                if (cell.Moves != null && cell.Moves.Count > 0) {
                    numMoves += cell.Moves.Count;
                }

                if (board.Board[row, col] == 0) {
                    numEmptyCells++;
                }
            }

            if (numEmptyCells == 0) {
                return Score.SolvedRegionScore;
            }

            return new MultiPartScore(new double[] { -numMoves, numEmptyCells });
        }

        public override IScore GetColScore(IBoardCells board, int col) {
            int numMoves = 0;
            int numEmptyCells = 0;
            for (int row = 0; row < board.Board.Size; row++) {
                var cell = MoveFinder.GetMovesForCell(board, row, col);

                if (cell.Moves != null) {
                    numMoves += cell.Moves.Count;
                }

                if (board.Board[row, col] == 0) {
                    numEmptyCells++;
                }
            }

            if (numEmptyCells == 0) {
                return Score.SolvedRegionScore;
            }

            return new MultiPartScore(new double[] { -numMoves, numEmptyCells });
        }

        public override IScore GetSquareScore(IBoardCells board, int sqRow, int sqCol) {
            int numMoves = 0;
            int numEmptyCells = 0;

            int sqSize = (int)Math.Sqrt(board.Board.Size);
            int row = sqRow * sqSize;
            int col = sqCol * sqSize;

            for (int r = row; r < row + sqSize; r++) {
                for (int c = col; c < col + sqSize; c++) {
                    var cell = MoveFinder.GetMovesForCell(board, r, c);
                    if (cell.Moves != null) {
                        numMoves += cell.Moves.Count;
                    }

                    if (board.Board[r, c] == 0) {
                        numEmptyCells++;
                    }
                }
            }

            if (numEmptyCells == 0) {
                return Score.SolvedRegionScore;
            }

            return new MultiPartScore(new double[] { -numMoves, numEmptyCells });
        }
    }
}
