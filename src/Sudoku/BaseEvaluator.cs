namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class BaseEvaluator : IEvaluator {
        public abstract IScore GetScore(IMove move);
        public abstract IScore GetScore(IBoard board);

        public bool HasMoves(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            // check that the board is valid and that there are cells with a 0

            // search for a cell with 0 and then return true
            for (int row = 0; row < board.Size; row++) {
                for (int col = 0; col < board.Size; col++) {
                    if (board[row, col] == 0) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsValid(IBoard board) {
            // todo: not sure if this method is needed or not
            throw new NotImplementedException();
        }
    }
}
