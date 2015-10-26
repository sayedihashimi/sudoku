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

            // for this basic implementation the score is 1/(# moves on board)
            IList<IMove> moves = MoveFinder.FindMoves(boardCells);

            if (moves == null || moves.Count == 0) {
                // board should be solved
                if (HasMoves(boardCells.Board)) {
                    // unsolvable board
                    return Score.MinScore;
                }

                // the board is solved
                return Score.MaxScore;
            }

            int numForcedMoves = 0;
            foreach(var move in moves) {
                if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    numForcedMoves++;
                }
            }

            double scorevalue = -1*(2 * (numForcedMoves) + (moves.Count - numForcedMoves));
            return new Score(scorevalue);
            // return new Score(-1.0d * moves.Count);
        }

        public override IScore GetScore(IMove move) {
            if(move.MoveScore == null) {
                move.MoveScore = GetScore(new Board(move.Board, move));
            }

            return move.MoveScore;
        }
    }
}
