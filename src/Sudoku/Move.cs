namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMove {
        IBoard Board { get; }
        int Row { get; }
        int Column { get; }
        int Value { get; }
    }

    public class Move : IMove  {

        public IBoard Board { get; }
        public int Row { get; }
        public int Column { get; }
        public int Value { get; }

        public Move(IBoard board, int row, int column, int value) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            Board = board;
            Row = row;
            Column = column;
            Value = value;
        }
    }
}
