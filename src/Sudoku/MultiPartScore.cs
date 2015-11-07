using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
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
            if (other != null) {
                return (this.CompareTo(other) == 0);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            int hashcode = 0;
            foreach (var item in Parts) {
                hashcode += item.GetHashCode();
            }
            return hashcode;
        }

        public static MultiPartScore operator +(MultiPartScore score1, MultiPartScore score2) {
            if (score1 == null && score2 == null) {
                return null;
            }

            int maxNumParts = 0;
            if (score1 != null) {
                maxNumParts = score1.Parts.Length;
            }

            if (score2 != null && score2.Parts.Length > maxNumParts) {
                maxNumParts = score2.Parts.Length;
            }

            double[] newParts = new double[maxNumParts];
            for (int i = 0; i < maxNumParts; i++) {
                if (score1 != null && score1.Parts.Length < i) {
                    newParts[i] += score1.Parts[i];
                }
                if (score2 != null && score2.Parts.Length < i) {
                    newParts[i] += score2.Parts[i];
                }
            }

            return new MultiPartScore(newParts);
        }

        public int CompareTo(object obj) {
            MultiPartScore other = (MultiPartScore)obj;

            if (other != null) {
                // compare the parts
                int numPartsThis = Parts.Length;
                int numPartsOther = other.Parts.Length;

                int minNumParts = Math.Min(Parts.Length, other.Parts.Length);

                for (int i = 0; i < minNumParts; i++) {
                    int result = Parts[i].CompareTo(other.Parts[i]);

                    if (result != 0) {
                        return result;
                    }
                }

                // all parts match through min number of elements
                if (numPartsThis == numPartsOther) {
                    return 0;
                }
                else {
                    if (numPartsThis > numPartsOther) {
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

            for (int index = 0; index < Parts.Length; index++) {
                if (index != 0) {
                    sb.Append(".");
                }
                sb.Append(Parts[index].ToString());
            }

            return sb.ToString();
        }
    }
}
