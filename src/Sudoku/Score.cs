namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IScore {
        int GetScore(IBoard board);
    }

    public class Score : IScore
    {
        public int GetScore(IBoard board) {
            throw new NotImplementedException();
        }
    }
}
