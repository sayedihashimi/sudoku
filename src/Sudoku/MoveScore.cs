namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMoveScore {
        int Score { get; }
        bool IsForcedMove { get;  }
    }

    public class MoveScore : IMoveScore {
        public MoveScore(int score):this(score, false) {

        }
        public MoveScore(int score,bool isForcedMove) {
            Score = score;
            IsForcedMove = IsForcedMove;
        }
        public bool IsForcedMove {
            get;
        }

        public int Score {
            get;
        }
    }
}
