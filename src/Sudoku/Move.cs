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
        IMoveScore Score { get; set; }
    }

    public class Move : IMove  {
        public IBoard Board { get; }
        public int Row { get; }
        public int Column { get; }
        public int Value { get; }

        public IMoveScore Score { get; set; }
        public Move(IBoard board, int row, int column, int value) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }

            Board = board;
            Row = row;
            Column = column;
            Value = value;
        }

        public override bool Equals(object obj) {
            IMove other = (IMove)obj;

            if(other != null) {
                if(other.Row != Row ||
                    other.Column != Column ||
                    other.Value != Value ||
                    !other.Board.Equals(Board)) {
                    return false;
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode() {
            return Row.GetHashCode() + Column.GetHashCode() + Value.GetHashCode() + Board.GetHashCode();
        }
    }
}
