namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBoard {
        int Size { get; }
        int this[int row, int column] { get; }
    }

    public class Board : IBoard {

        private int[,] _data;

        public Board(int[,] data) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            Validate(data);
            _data = (int[,])data.Clone();
        }

        public Board(int[,] data, IMove move) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            _data = (int[,])data.Clone();
            _data[move.Row, move.Column] = move.Value;

            Validate(_data);
        }

        public Board(IBoard board) : this(board, null) {
        }

        public Board(IBoard board, IMove move) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            Board other = (Board)board;

            if (other != null) {
                _data = (int[,])other._data.Clone();

                if (move != null) {
                    _data[move.Row, move.Column] = move.Value;
                }
            }
            else {
                throw new ArgumentException($"Unknown board type {nameof(board)}");
            }

            Validate(_data);
        }

        /// <summary>
        /// Only works for boards with size 9
        /// </summary>
        /// <param name="dataStr"></param>
        public Board(string dataStr) : this(new Board(GetDataFromString(dataStr))) {
        }

        private static int[,] GetDataFromString(string dataStr) {
            if (string.IsNullOrEmpty(dataStr)) { throw new ArgumentNullException(nameof(dataStr)); }

            dataStr = dataStr.Trim();
            int strLen = dataStr.Length;
            if (dataStr.Length != 81) {
                throw new ArgumentException("Expected a string with 81 characters");
            }
            int[,] data = new int[9, 9];
            var chars = dataStr.ToArray();

            int index = 0;
            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    string intStr = new string(new char[] { chars[index] });
                    if (intStr.Equals(".")) {
                        intStr = "0";
                    }
                    data[row, col] = int.Parse(intStr);
                    index++;
                }
            }

            return data;
        }

        public int Size {
            get {
                return _data.GetLength(0);
            }
        }

        public int this[int row, int column] {
            get {
                return _data[row, column];
            }
        }

        public static void Validate(int[,] data) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            if (data.Rank != 2) {
                throw new ArgumentException($"Rank of the array was expected to be 2, but was {data.Rank}");
            }

            if (data.GetLength(0) != data.GetLength(1)) {
                throw new ArgumentException($"Dimensions of the array do not match [{data.GetLength(0)},{data.GetLength(1)}]");
            }

            // check that each cell value is <= board.Size
            int size = data.GetLength(0);
            // validate that the sqroot is an integer
            var sqrt = Math.Sqrt(size);
            if (Math.Abs(Math.Ceiling(sqrt) - Math.Floor(sqrt)) > Double.Epsilon) {
                throw new ArgumentException($"Unexpected side length [{size}]. The square root must be an even number");
            }

            for (int rowIndex = 0; rowIndex < size; rowIndex++) {
                for (int colIndex = 0; colIndex < size; colIndex++) {
                    // ensure the cell value is <= board.Size and >= 0
                    if (data[rowIndex, colIndex] > size || data[rowIndex, colIndex] < 0) {
                        throw new ArgumentException($"Cell value at [{rowIndex},{colIndex}] is invalid, max allowed=[{size}]");
                    }
                }
            }

            // check for duplicates in the row/column and later in the square itself
            for (int i = 0; i < size; i++) {
                int[] numUsedInRow = new int[size];
                int[] numUsedInCol = new int[size];

                for (int j = 0; j < size; j++) {
                    int value = data[i, j];
                    if (value == 0) { continue; }

                    if (numUsedInRow.Contains(value)) {
                        throw new InvalidBoardDataException($"Duplicate value [{value}] found in row in cell [{i},{j}]");
                    }
                    else {
                        numUsedInRow[value - 1] = value;
                    }

                    if (numUsedInCol.Contains(value)) {
                        throw new InvalidBoardDataException($"Duplicate value [{value}] found in row in cell [{j},{i}]");
                    }
                    else {
                        numUsedInCol[value - 1] = value;
                    }
                }
            }

            // check each square for duplicates
            foreach(int[,]square in GetSquares(data)) {
                // check for duplicates in the square
                int[] numbersUsed = new int[size];

                foreach(int num in square) {
                    if (num == 0) { continue; }

                    if (numbersUsed[num - 1] != 0) {
                        throw new InvalidBoardDataException($"Duplicate value [{num}] found");
                    }
                    else {
                        numbersUsed[num - 1] = num;
                    }
                }
            }
        }

        public static IList<int[,]> GetSquares(int[,]data) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            int boardsize = data.GetLength(0);
            int squaresize = (int)Math.Sqrt(boardsize);
            int size = boardsize;

            int [,][,]_squaresData = new int[squaresize, squaresize][,];
            for (int i = 0; i < squaresize; i++) {
                for (int j = 0; j < squaresize; j++) {
                    _squaresData[i, j] = new int[squaresize, squaresize];
                }
            }

            for (int row = 0; row < size; row++) {
                for (int col = 0; col < size; col++) {
                    int sqRowIndex = (int)Math.Floor((double)(row / squaresize));
                    int sqColIndex = (int)Math.Floor((double)(col / squaresize));

                    int subrow = row - sqRowIndex * squaresize;
                    int subcol = col - sqColIndex * squaresize;
                    _squaresData[sqRowIndex, sqColIndex][subrow, subcol] = data[row, col];
                }
            }

            IList<int[,]> squareList = new List<int[,]>();
            for (int i = 0; i < squaresize; i++) {
                for (int j = 0; j < squaresize; j++) {
                    squareList.Add(_squaresData[i, j]);
                }
            }

            return squareList;
        }

        public override bool Equals(object obj) {
            Board other = (Board)obj;
            if (other != null) {
                if (other.Size != Size ||
                    other._data.Rank != _data.Rank ||
                    other._data.GetLength(0) != _data.GetLength(0)) {
                    return false;
                }

                for (int i = 0; i < Size; i++) {
                    for (int j = 0; j < Size; j++) {
                        if (_data[i, j] != other._data[i, j]) {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public override int GetHashCode() {
            return Size.GetHashCode() + _data.GetHashCode();
        }

        public string ToFlatString() {
            var sb = new StringBuilder(Size * Size);
            for (int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    sb.Append(_data[row, col].ToString());
                }
            }
            return sb.ToString();
        }

        public override string ToString() {
            var gridSize = _data.GetLength(0);
            var sb = new StringBuilder(Size * Size * Size);

            for (int row = 0; row < Size; row++) {
                for (int col = 0; col < Size; col++) {
                    if(col>0 && row < gridSize) {
                        sb.Append(",");
                    }
                    sb.Append(_data[row, col].ToString());
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
