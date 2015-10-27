namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMoveFinder {
        List<IMove> FindMoves(IBoardCells boardCells);
    }

    public class SimpleMoveFinder : IMoveFinder {
        public List<IMove> FindMoves(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            List<IMove> moves = new List<IMove>();
            // visit each cell and get available moves
            for (int row = 0; row < boardCells.Board.Size; row++) {
                for (int col = 0; col < boardCells.Board.Size; col++) {
                    var cellMoves = GetMovesForCell(boardCells, row, col);
                    foreach (var move in cellMoves) {
                        if (!moves.Contains(move)) {
                            moves.Add(move);
                        }
                    }
                }
            }

            return moves;
        }

        public IList<IMove> GetMovesForCell(IBoardCells boardCells, int row, int col) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            List<IMove> moves = new List<IMove>();

            if (boardCells.Board[row, col] == 0) {
                int[] availableNumbers = new int[boardCells.Board.Size];
                for (int i = 0; i < boardCells.Board.Size; i++) {
                    availableNumbers[i] = i + 1;
                }

                foreach (int num in boardCells.GetRowForCell(row, col)) {
                    if (num == 0) { continue; }
                    availableNumbers[num - 1] = 0;
                }

                foreach (int num in boardCells.GetColumnForCell(row, col)) {
                    if (num == 0) { continue; }
                    availableNumbers[num - 1] = 0;
                }

                foreach (int num in boardCells.GetSquareForCell(row, col)) {
                    if (num == 0) { continue; }
                    availableNumbers[num - 1] = 0;
                }

                foreach (int num in availableNumbers) {
                    if (num == 0) { continue; }
                    moves.Add(new Move(boardCells.Board, row, col, num));
                }

                // if there is only one move for a cell then it's forced
                if (moves.Count == 1) {
                    moves[0].IsForcedMove = true;
                }
            }
            return moves;
        }
    }
}
