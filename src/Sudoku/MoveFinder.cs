namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMoveFinder {
        List<CellMoves> FindMoves(IBoardCells boardCells);

        CellMoves GetMovesForCell(IBoardCells boardCells, int row, int col);
    }

    public class CellMoves {
        public CellMoves(int row, int col, List<IMove> moves) {
            Row = row;
            Col = col;
            Moves = moves;
        }
        public int Row { get; }
        public int Col { get; }
        public List<IMove> Moves { get; set; }
    }

    public class SimpleMoveFinder : IMoveFinder {
        public List<CellMoves> FindMoves(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            var moves = new List<CellMoves>();
            // visit each cell and get available moves
            for (int row = 0; row < boardCells.Board.Size; row++) {
                for (int col = 0; col < boardCells.Board.Size; col++) {
                    var cellMoves = GetMovesForCell(boardCells, row, col);
                    if (cellMoves.Moves.Count > 0) {
                        moves.Add(cellMoves);
                    }
                }
            }

            return moves;
        }

        public CellMoves GetMovesForCell(IBoardCells boardCells, int row, int col) {
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
            return new CellMoves(row, col, moves);
        }
    }
}
