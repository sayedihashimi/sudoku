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

    public class MultiPartScore : IScore {

        public MultiPartScore(double[] parts) {
            if (parts == null || parts.Length == 0) { throw new ArgumentNullException(nameof(parts)); }
            
            Parts = parts;
        }
        private double[] Parts {
            get;
        }

        public override bool Equals(object obj) {
            MultiPartScore other = (MultiPartScore)obj;
            if(other != null) {
                return (this.CompareTo(other) == 0);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            int hashcode = 0;
            foreach(var item in Parts) {
                hashcode += item.GetHashCode();
            }
            return hashcode;
        }

        // Returns:
        //     A value that indicates the relative order of the objects being compared. The
        //     return value has these meanings: Value Meaning Less than zero This instance precedes
        //     obj in the sort order. Zero This instance occurs in the same position in the
        //     sort order as obj. Greater than zero This instance follows obj in the sort order.
        public int CompareTo(object obj) {
            MultiPartScore other = (MultiPartScore)obj;

            if (other != null) {
                // compare the parts
                int numPartsThis = Parts.Length;
                int numPartsOther = other.Parts.Length;

                int minNumParts = Math.Min(Parts.Length, other.Parts.Length);

                for(int i = 0; i < minNumParts; i++) {
                    int result = Parts[i].CompareTo(other.Parts[i]);

                    if(result != 0) {
                        return result;
                    }
                }

                // all parts match through min number of elements
                if(numPartsThis == numPartsOther) {
                    return 0;
                }
                else {
                    if(numPartsThis > numPartsOther) {
                        return 1;
                    }
                    else {
                        return -1;
                    }
                }
            }
            else {
                throw new ArgumentException($"Expected an object of type [{typeof(MultiPartScore)}]");
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            for(int index = 0; index < Parts.Length; index++) {
                if(index != 0) {
                    sb.Append(".");
                }
                sb.Append(Parts[index].ToString());
            }

            return sb.ToString();
        }
    }
}
