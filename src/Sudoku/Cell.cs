using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku {
    public class Cell {
        public Cell(int row, int col, List<IMove> moves) {
            Row = row;
            Col = col;
            Moves = moves;
        }
        public int Row { get; }
        public int Col { get; }
        public List<IMove> Moves { get; set; }
    }
}
