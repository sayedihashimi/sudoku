namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class MultiPartScoreTests {

        [Fact]
        public void WillReturn0ForEqual() {
            MultiPartScore one = new MultiPartScore(new double[] { 1.1d, 2.1d });
            MultiPartScore two = new MultiPartScore(new double[] { 1.1d, 2.1d });
            Assert.Equal(0, one.CompareTo(two));
            Assert.Equal(0, two.CompareTo(one));
        }

        [Fact]
        public void WillReturnCorrectValueForEqualNumberOfParts01() {
            MultiPartScore one = new MultiPartScore(new double[] { 2.1d, 2.1d });
            MultiPartScore two = new MultiPartScore(new double[] { 1.1d, 2.1d });
            Assert.Equal(1, one.CompareTo(two));
            Assert.Equal(-1, two.CompareTo(one));
        }

        [Fact]
        public void WillReturnCorrectValueForEqualNumberOfParts02() {
            MultiPartScore one = new MultiPartScore(new double[] { 1.1d, 2.2d });
            MultiPartScore two = new MultiPartScore(new double[] { 1.1d, 2.1d });
            Assert.Equal(1, one.CompareTo(two));
            Assert.Equal(-1, two.CompareTo(one));
        }

        [Fact]
        public void WillReturnCorrectValueForEqualNumberOfParts3() {
            MultiPartScore one = new MultiPartScore(new double[] { 1.1d, 2.2d, 0.5d, 57.9d, 100.545d });
            MultiPartScore two = new MultiPartScore(new double[] { 1.1d, 2.2d, 0.5d, 56.9d, 100.545d });
            Assert.Equal(1, one.CompareTo(two));
            Assert.Equal(-1, two.CompareTo(one));
        }
    }
}
