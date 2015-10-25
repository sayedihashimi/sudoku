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

            // for this basic implementation the score is 1/(# moves on board)
            IList<IMove> moves = MoveFinder.FindMoves(new BoardCells(board));
            
            if(moves == null || moves.Count == 0) {
                // board should be solved
                if (HasMoves(board)) {
                    throw new InvalidOperationException("No moves found for board but the board has moves left");
                }

                // the board is solved
                return Score.MaxScore;
            }

            return new Score(1 / (moves.Count));
        }

        public override IScore GetScore(IMove move) {
            if(move.MoveScore == null) {
                move.MoveScore = GetScore(new Board(move.Board, move));
            }

            return move.MoveScore;
        }
    }
}
