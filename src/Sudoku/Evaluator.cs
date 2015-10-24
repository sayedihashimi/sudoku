namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEvaluator {
        /// <summary>
        /// This will return the score for the given board
        /// </summary>
        IBoardScore GetScore(IBoard board);

        /// <summary>
        /// Will return true if the board is in a valid state, this will not check to see if the board is "solveable"
        /// but instead just checks for duplicate numbers in rows/columns/squares
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        bool IsValid(IBoard board);

        /// <summary>
        /// Will return true if the board is valid and has moves available
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        bool HasMoves(IBoard board);
    }

    public abstract class BaseEvaluator : IEvaluator {
        public abstract IBoardScore GetScore(IBoard board);

        public bool HasMoves(IBoard board) {
            // check that the board is valid and that there are cells with a 0

            throw new NotImplementedException();
        }

        public bool IsValid(IBoard board) {
            // check to make sure there are no duplicates in rows/columns/squares
            // check to make sure every cell value is <= board.Size

            throw new NotImplementedException();
        }
    }
}