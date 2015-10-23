namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMoveFinder {
        IList<IMove> FindMoves(IBoardCells boardCells);
    }

    public class SimpleMoveFinder : IMoveFinder {
        public IList<IMove> FindMoves(IBoardCells boardCells) {
            if (boardCells == null) { throw new ArgumentNullException(nameof(boardCells)); }
            
            
            // visit each cell and get available moves


            throw new NotImplementedException();
        }
    }

}
