﻿namespace Sudoku {
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
                return Score.MaxScore;
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
                        return Score.MinScore;
                    }

                    // the board is solved
                    return Score.MaxScore;
                }

                double score = -1 * numMoves;
                if (numForcedMoves > 0) {
                    score += ((double)numForcedMoves / (double)numMoves) * 0.5;
                }
                boardCells.BoardScore = new Score(score);
                // return new Score(score);
                // return new Score(1.0d/(1+numMoves + (numMoves-numForcedMoves)));
            }
            else {
                var foo = "bar";
            }

            return boardCells.BoardScore;
        }

        public override IScore GetScore(IMove move) {
            if(move.MoveScore == null) {
                if (Board.IsSolved((Board)move.Board)) {
                    return Score.MaxScore;
                }

                if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    move.MoveScore = new Score(Score.MaxScore.ScoreValue - 10d);
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
                                numEmptyCells++;
                            }
                        }
                    }

                    move.MoveScore = new Score(numEmptyCells + 1.0d/(1.0d+score));
                }
            }

            return move.MoveScore;
        }
    }
}
