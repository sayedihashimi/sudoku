namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBoardCells {
        IBoard Board { get; }
        IList<int[]> Rows { get;}
        IList<int[]> Columns { get; }
        IList<int[,]> Squares { get; }
        IScore BoardScore { get; set; }

        int[] GetRowForCell(int row, int col);

        int[] GetColumnForCell(int row, int col);

        int[,] GetSquareForCell(int row, int col);

        int[,] GetSquare(int squareRow, int squareCol);
    }

    public class BoardCells : IBoardCells {
        public BoardCells(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            Board = board;
            Rows = GetRows(Board);
            Columns = GetColumns(Board);
            Squares = GetSquares(Board);
        }

        public BoardCells(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }

            Board = boardCells.Board;
            Rows = boardCells.Rows;
            Columns = boardCells.Columns;
            Squares = boardCells.Squares;

            BoardCells other = (BoardCells)boardCells;
            if(other != null) {
                _squaresData = other._squaresData;
            }
        }

        public IBoard Board { get; }

        public IList<int[]> Rows { get; }
        public IList<int[]> Columns { get; }
        public IList<int[,]> Squares { get; }
        public IScore BoardScore { get; set; }
        protected internal int[,][,] _squaresData;

        protected internal IList<int[]> GetRows(IBoard board) {
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
        protected internal IList<int[]> GetColumns(IBoard board) {
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

        protected internal IList<int[,]> GetSquares(IBoard board) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            int squaresize = (int)Math.Sqrt(board.Size);
            int size = board.Size;

            IList<int[,]> squareList = new List<int[,]>();

            _squaresData = new int[squaresize, squaresize][,];
            for(int i = 0; i < squaresize; i++) {
                for(int j = 0; j < squaresize; j++) {
                    _squaresData[i, j] = new int[squaresize, squaresize];
                }
            }

            for (int row = 0; row < size; row++) {
                for (int col = 0; col < size; col++) {
                    int sqRowIndex = (int)Math.Floor((double)(row / squaresize));
                    int sqColIndex = (int)Math.Floor((double)(col / squaresize));

                    int subrow = row - sqRowIndex * squaresize;
                    int subcol = col - sqColIndex * squaresize;
                    _squaresData[sqRowIndex, sqColIndex][subrow, subcol] = board[row,col];
                }
            }            

            for(int i = 0; i < squaresize; i++) {
                for(int j = 0; j < squaresize; j++) {
                    squareList.Add(_squaresData[i, j]);
                }
            }

            return squareList;
        }

        public int[] GetRowForCell(int row,int col) {
            return Rows[row];
        }

        public int[] GetColumnForCell(int row, int col) {
            return Columns[col];
        }

        public int[,] GetSquareForCell(int row, int col) {
            // compute the square index and then the subindex
            int squaresize = (int)Math.Sqrt(Board.Size);
            int sqRowIndex = (int)(Math.Floor((double)(row / squaresize)));
            int sqColIndex = (int)(Math.Floor((double)(col / squaresize)));

            return _squaresData[sqRowIndex, sqColIndex];
        }

        public int[,] GetSquare(int squareRow, int squareCol) {
            return _squaresData[squareRow, squareCol];
        }
    }
}
