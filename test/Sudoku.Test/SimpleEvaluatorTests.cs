namespace Sudoku.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleEvaluatorTests {
        [Fact]
        public void WillReturnMaxScoreForSolvedBoard() {
            int[,] data = new int[,] {
                {9,6,2,7,1,5,8,3,4},
                {4,7,8,9,6,3,5,1,2},
                {3,1,5,4,2,8,6,9,7},
                {1,2,7,6,5,9,3,4,8},
                {8,9,4,2,3,1,7,5,6},
                {5,3,6,8,7,4,9,2,1},
                {6,5,3,1,4,7,2,8,9},
                {7,8,1,3,9,2,4,6,5},
                {2,4,9,5,8,6,1,7,3}
            };

            var eval = new SimpleEvaluator(new SimpleMoveFinder());
            var score =eval.GetScore(new Board(data));

            Assert.Equal(double.MaxValue, score.ScoreValue);
        }
    }
}
