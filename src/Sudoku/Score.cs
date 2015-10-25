namespace Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface represents the score for a given board or move.
    /// The higher the number the better the score
    /// Values can be any range of values including negative
    /// </summary>
    public interface IScore {
        double ScoreValue { get; }
    }

    public class Score : IScore {
        public Score(double score) {
            ScoreValue = score;
        }
        public double ScoreValue {
            get;
        }

        public static Score MaxScore {
            get {
                return new Score(double.MaxValue);
            }
        }

        public override bool Equals(object obj) {
            IScore other = (IScore)obj;
            if(other != null) {
                return ScoreValue.Equals(other.ScoreValue);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return ScoreValue.GetHashCode();
        }
    }
}
