namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBoardCells {
        IList<int[]> GetRows(IBoard board);
        IList<int[]> GetColumns(IBoard board);
        IList<int[,]> GetSquares(IBoard board);
    }

    public class BoardCells : IBoardCells {
        public IList<int[]> GetRows(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            IList<int[]> rows = new List<int[]>();
            for (int rowIndex = 0; rowIndex < board.Size; rowIndex++) {
                var row = new int[board.Size];
                for (int colIndex = 0; colIndex < board.Size; colIndex++) {
                    row[colIndex] = board[rowIndex, colIndex];
                }

                rows.Add(row);
            }

            return rows;
        }
        public IList<int[]> GetColumns(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            IList<int[]> cols = new List<int[]>();
            for (int colIndex = 0; colIndex < board.Size; colIndex++) {
                var col = new int[board.Size];
                for (int rowIndex = 0; rowIndex < board.Size; rowIndex++) {
                    col[rowIndex] = board[rowIndex, colIndex];
                }

                cols.Add(col);
            }

            return cols;
        }

        public IList<int[,]> GetSquares(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            int squaresize = (int)Math.Sqrt(board.Size);
            int size = board.Size;

            IList<int[,]> squareList = new List<int[,]>();

            int[,][,] squares = new int[squaresize, squaresize][,];
            for(int i = 0; i < squaresize; i++) {
                for(int j = 0; j < squaresize; j++) {
                    squares[i, j] = new int[squaresize, squaresize];
                }
            }

            for (int row = 0; row < size; row++) {
                for (int col = 0; col < size; col++) {
                    int sqRowIndex = (int)Math.Floor((double)(row / squaresize));
                    int sqColIndex = (int)Math.Floor((double)(col / squaresize));

                    int subrow = row - sqRowIndex * squaresize;
                    int subcol = col - sqColIndex * squaresize;
                    squares[sqRowIndex, sqColIndex][subrow, subcol] = board[row,col];
                }
            }            

            for(int i = 0; i < squaresize; i++) {
                for(int j = 0; j < squaresize; j++) {
                    squareList.Add(squares[i, j]);
                }
            }

            return squareList;
        }
    }
}
