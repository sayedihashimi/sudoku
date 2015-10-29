namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SimplePlayer : IPlayer {

        public SimplePlayer(IMoveFinder moveFinder,IEvaluator evaluator) {
            if (moveFinder == null) { throw new ArgumentNullException(nameof(moveFinder)); }

            MoveFinder = moveFinder;
            Evaluator = evaluator;
            _numMovesTried = 0;
        }
        public int _numMovesTried;
        private IMoveFinder MoveFinder { get; }
        private IEvaluator Evaluator { get; }

        public MoveResult SolveBoard(IBoard board) {
            var bc = new BoardCells(board);
            var result = SolveBoard(new MoveResult(bc,new List<IMove>(),null));

            return result;
        }

        public MoveResult SolveBoard(MoveResult board) {
            if(board == null) {
                return null;
            }

            List<IMove> movesRemaining = board.MovesRemaining;
            if (movesRemaining == null) {
                movesRemaining = MoveFinder.FindMoves(board.CurrentBoard);
            }

            if(movesRemaining.Count <= 0) {
                if (IsSolved(board)) {
                    return board;
                }
                else {
                    return null;
                }
            }

            MoveResult currentBoard = null;
            try{
                currentBoard = PlayForcedMoves(board.CurrentBoard, movesRemaining);
            }
            catch (InvalidBoardDataException) {
                // unsolveable board
                return null;
            }

            movesRemaining = currentBoard.MovesRemaining;
            List<IMove> movesPlayed = currentBoard.MovesPlayed;

            // make sure each score has a movescore
            foreach(var move in movesRemaining) {
                if(move.MoveScore == null) {
                    move.MoveScore = Evaluator.GetScore(move);
                }
            }

            // sort the moves remaining before starting
            movesRemaining = movesRemaining.OrderBy(move => move.MoveScore.ScoreValue*(-1.0d)).ToList();
            if (movesRemaining.Count > 0) {
                // now play the remaining moves
                foreach (var move in movesRemaining) {
                    if (move.MoveScore.Equals(Score.MinScore)) {
                        // don't play failed moves
                        continue;
                    }

                    movesPlayed.Add(move);
                    var newmv = new MoveResult(new BoardCells(new Board(currentBoard.CurrentBoard.Board, move)), movesPlayed, null);

                    if (move.MoveScore.Equals(Score.MaxScore)) {
                        return newmv;
                    }

                    _numMovesTried++;
                    var newresult = SolveBoard(newmv);                    
                    if (newresult != null && IsSolved(newresult)) {
                        // see if it's solved
                        return newresult;
                    }
                    movesPlayed.Remove(move);
                }

                return null;
            }

            if (IsSolved(currentBoard)) {
                return currentBoard;
            }
            else {
                return null;
            }
        }

        protected internal bool IsSolved(MoveResult moveResult) {
            if (moveResult == null) { throw new ArgumentNullException(nameof(moveResult)); }
            var score = Evaluator.GetScore(moveResult.CurrentBoard);
            return Score.MaxScore.Equals(score);
        }
        /// <summary>
        /// Will play all forced moves. The result returned will be the resulting
        /// board and an unsorted move list which does not have any forced moves.
        /// </summary>
        protected MoveResult PlayForcedMoves(IBoardCells board, List<IMove> moves) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            if (moves == null) { throw new ArgumentNullException(nameof(moves)); }

#if DEBUG
            var actualMoves = MoveFinder.FindMoves(board);
            if(actualMoves.Count != moves.Count) {
                string foo = "bar";
            }
#endif
            IBoardCells result = new BoardCells(board);
            IBoard playboard = result.Board;
            List<IMove> movesPlayed = new List<IMove>();
            List<IMove> forcedMoves = GetForcedMoves(moves);
            do {
                foreach(var move in forcedMoves) {
                    playboard = new Board(playboard, move);
                    movesPlayed.Add(move);
                    _numMovesTried++;
                }

                
                forcedMoves = GetForcedMoves(MoveFinder.FindMoves(new BoardCells(playboard)));
            } while (forcedMoves.Count > 0);

            result = new BoardCells(playboard);
            return new MoveResult(result, movesPlayed, MoveFinder.FindMoves(result));
        }

        protected List<IMove> GetForcedMoves(IList<IMove>moves) {
            if (moves == null) { throw new ArgumentNullException(nameof(moves)); }

            List<IMove> forcedMoves = new List<IMove>();
            foreach(var move in moves) {
                if(move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    forcedMoves.Add(move);
                }
            }

            return forcedMoves;
        }
    }
}
