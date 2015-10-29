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

            // check if the board is solved and return if so
            if (IsSolved(board)) {
                return board;
            }

            List<IMove> movesPlayed = new List<IMove>();
            if(board.MovesPlayed != null && board.MovesPlayed.Count > 0) {
                movesPlayed.AddRange(board.MovesPlayed);
            }

            List<CellMoves> boardmoves = MoveFinder.FindMoves(board.CurrentBoard);
            // sort each cell by # of cell moves
            boardmoves = boardmoves.OrderBy(bm => bm.Moves.Count).ToList();

            foreach(var cell in boardmoves) {                
                if (cell.Moves.Count > 0 && board.CurrentBoard.Board[cell.Row, cell.Col] == 0) {
                    var cellMoves = MoveFinder.GetMovesForCell(board.CurrentBoard, cell.Row, cell.Col);

                    if (cellMoves.Moves.Count == 0) {
                        // unsolveable
                        return null;
                    }

                    foreach (var move in cellMoves.Moves) {
                        if (move.MoveScore == null) {
                            move.MoveScore = Evaluator.GetScore(move);
                        }
                    }
                    cellMoves.Moves = cellMoves.Moves.OrderBy(move => move.MoveScore.ScoreValue * (-1.0d)).ToList();
                    foreach (var move in cellMoves.Moves) {
                        movesPlayed.Add(move);
                        var newmv = new MoveResult(new BoardCells(new Board(board.CurrentBoard.Board, move)), movesPlayed, null);

                        _numMovesTried++;
                        var newresult = SolveBoard(newmv);
                        if (newresult != null && IsSolved(newresult)) {
                            return newresult;
                        }

                        movesPlayed.Remove(move);
                    }

                    // unsolveable
                    // return null;
                }
            }

            //// for each cell which has a zero value and play it
            //for(int row = 0; row < board.CurrentBoard.Board.Size; row++) {
            //    for(int col = 0; col < board.CurrentBoard.Board.Size; col++) {
            //        if(board.CurrentBoard.Board[row,col] == 0) {
            //            var cellMoves = MoveFinder.GetMovesForCell(board.CurrentBoard, row, col);

            //            if(cellMoves.Moves.Count == 0) {
            //                // unsolveable
            //                return null;
            //            }

            //            foreach (var move in cellMoves.Moves) {
            //                if (move.MoveScore == null) {
            //                    move.MoveScore = Evaluator.GetScore(move);
            //                }
            //            }
            //            cellMoves.Moves = cellMoves.Moves.OrderBy(move => move.MoveScore.ScoreValue * (-1.0d)).ToList();
            //            foreach (var move in cellMoves.Moves) {
            //                movesPlayed.Add(move);
            //                var newmv = new MoveResult(new BoardCells(new Board(board.CurrentBoard.Board, move)), movesPlayed, null);
                            
            //                _numMovesTried++;
            //                var newresult = SolveBoard(newmv);
            //                if (newresult != null && IsSolved(newresult)) {
            //                    return newresult;
            //                }

            //                movesPlayed.Remove(move);
            //            }

            //            // unsolveable
            //            // return null;
            //        }
            //    }
            //}

            if (IsSolved(board)) {
                return board;
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
//        protected MoveResult PlayForcedMoves(IBoardCells board, List<IMove> moves) {
//            if (board == null) { throw new ArgumentNullException(nameof(board)); }
//            if (moves == null) { throw new ArgumentNullException(nameof(moves)); }

//#if DEBUG
//            var actualMoves = MoveFinder.FindMoves(board);
//            if(actualMoves.Count != moves.Count) {
//                string foo = "bar";
//            }
//#endif
//            IBoardCells result = new BoardCells(board);
//            IBoard playboard = result.Board;
//            List<IMove> movesPlayed = new List<IMove>();
//            List<IMove> forcedMoves = GetForcedMoves(moves);
//            do {
//                foreach(var move in forcedMoves) {
//                    playboard = new Board(playboard, move);
//                    movesPlayed.Add(move);
//                    _numMovesTried++;
//                }

                
//                forcedMoves = GetForcedMoves(MoveFinder.FindMoves(new BoardCells(playboard)));
//            } while (forcedMoves.Count > 0);

//            result = new BoardCells(playboard);
//            return new MoveResult(result, movesPlayed, MoveFinder.FindMoves(result));
//        }

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
