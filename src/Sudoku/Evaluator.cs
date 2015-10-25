﻿namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEvaluator {
        /// <summary>
        /// This will return the score for the given board
        /// </summary>
        IScore GetScore(IBoard board);

        /// <summary>
        /// This will return the score for the given move
        /// </summary>
        IScore GetScore(IMove move);

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


}