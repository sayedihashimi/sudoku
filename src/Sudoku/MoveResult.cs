namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MoveResult {
        public MoveResult(IBoardCells board, List<IMove> movesPlayed, List<IMove> movesRemaining) {
            CurrentBoard = board;
            MovesPlayed = movesPlayed;
            MovesRemaining = movesRemaining;
        }
        public IBoardCells CurrentBoard { get; }
        public List<IMove> MovesPlayed { get; }
        public List<IMove> MovesRemaining { get; }
    }
}
