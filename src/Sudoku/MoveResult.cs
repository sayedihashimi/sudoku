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

        public MoveResult(IBoardCells board, List<IMove> movesPlayed, List<Cell> cellMoves) : this(board,movesPlayed, (List<IMove>)null) {
            MovesRemaining = new List<IMove>();

            if(cellMoves != null) {
                foreach(var cell in cellMoves) {
                    if (cell.Moves != null) {
                        foreach (var move in cell.Moves) {
                            MovesRemaining.Add(move);
                        }
                    }
                }
            }            
        }

        public IBoardCells CurrentBoard { get; }
        public List<IMove> MovesPlayed { get; }
        public List<IMove> MovesRemaining { get; }
    }
}
