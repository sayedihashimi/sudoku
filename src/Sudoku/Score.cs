namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface represents the score for a given board or move.
    /// The higher the number the better the score
    /// Values can be any range of values including negative
    /// </summary>
    public interface IScore : IComparable {
        
    }
    
    public static class Score {
        public static IScore EmptyScore {
            get {
                return new MultiPartScore(new double[1] { 0.0d });
            }
        }
        public static IScore InvalidBoardScore {
            get {
                return new MultiPartScore(new double[1] { double.MinValue });
            }
        }

        public static IScore SolvedBoardScore {
            get {
                return new MultiPartScore(new double[1] { 10000 });
            }
        }

        public static IScore ForcedMoveScore {
            get {
                return new MultiPartScore(new double[1] { 1000 });
            }
        }

        public static IScore SolvedRegionScore {
            get {
                return new MultiPartScore(new double[1] { 1000 });
            }
        }
    }


}
