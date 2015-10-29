namespace Sudoku
{
    using System;

    public class InvalidBoardDataException : Exception {
        public InvalidBoardDataException() { }
        public InvalidBoardDataException(string message) : base(message) { }
        public InvalidBoardDataException(string message, Exception inner) : base(message, inner) { }
    }
}
