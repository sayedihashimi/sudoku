namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IPlayer {
        MoveResult SolveBoard(IBoard board);
    }
}
