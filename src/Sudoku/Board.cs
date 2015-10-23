namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBoard
    {
        int Size { get; }
        int this[int row,int column] { get; }
    }

    public class Board : IBoard {

        private int[,] _data;

        public Board(int[,] data) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            // validate that the length on each dimension matches and not empty
            if (data.Length <= 0) {
                throw new ArgumentException("Array provided for data is empty");
            }

            if(data.GetLength(0) != data.GetLength(1)) {
                throw new ArgumentException("Array provided for data has different lengths for rows and columns");
            }

            Validate(data);
            _data = (int[,])data.Clone();
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

        protected internal void Validate(int[,] data) {
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

            for(int rowIndex = 0; rowIndex < size; rowIndex++) {
                for(int colIndex = 0; colIndex < size; colIndex++) {
                    // ensure the cell value is <= board.Size and >= 0
                    if(data[rowIndex,colIndex] > size || data[rowIndex,colIndex] < 0) {
                        throw new ArgumentException($"Cell value at [{rowIndex},{colIndex}] is invalid, max allowed=[{size}]");
                    }
                }
            }
        }
    }
}
