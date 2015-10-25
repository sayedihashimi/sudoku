namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}
