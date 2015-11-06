namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SimplePlayer : IPlayer {

        public SimplePlayer(IMoveFinder moveFinder, IEvaluator evaluator) {
            if (moveFinder == null) { throw new ArgumentNullException(nameof(moveFinder)); }

            MoveFinder = moveFinder;
            Evaluator = evaluator;
            _numMovesTried = 0;
        }
        public int _numMovesTried;
        private IMoveFinder MoveFinder { get; }
        private IEvaluator Evaluator { get; }

        public MoveResult SolveBoard(IBoard board) {
            var result = SolveBoard(new MoveResult(new BoardCells(board), new List<IMove>(), (List<IMove>)null));

            return result;
        }

        public MoveResult SolveBoard(MoveResult board) {
            if (board == null) {
                return null;
            }

            // check if the board is solved and return if so
            if (IsSolved(board)) {
                return board;
            }
            if (!Board.IsValid((Board)board.CurrentBoard.Board)) {
                return null;
            }

            List<IMove> movesPlayed = new List<IMove>();
            if (board.MovesPlayed != null && board.MovesPlayed.Count > 0) {
                movesPlayed.AddRange(board.MovesPlayed);
            }

            board = PlayForcedMoves(board.CurrentBoard, Board.GetMovesFrom(MoveFinder.FindMoves(board.CurrentBoard)));

            if (board == null) {
                // invalid board
                return null;
            }

            if (IsSolved(board)) {
                return board;
            }

            List<Cell> boardmoves = MoveFinder.FindMoves(board.CurrentBoard);
            // make sure each move has a score
            if (boardmoves != null) {
                foreach (var bm in boardmoves) {
                    foreach (var mv in bm.Moves) {
                        if (mv.MoveScore == null) {
                            mv.MoveScore = Evaluator.GetScore(mv);
                        }
                    }
                }
            }

            if (boardmoves != null && boardmoves.Count > 1) {
                // sort each cell by # of cell moves
                // boardmoves = boardmoves.OrderBy(bm => bm.Moves.Count).ToList();

                boardmoves = boardmoves.OrderByDescending(bm => {
                    double score = 0;
                    score = -bm.Moves.Count;
                    double movescore = 0.0d;
                    foreach (var m in bm.Moves) {
                        movescore += m.MoveScore.ScoreValue;
                    }

                    return score - ((1.0d) / movescore);
                }).ToList();
            }

            foreach (var cell in boardmoves) {
                if (cell.Moves.Count > 0 && board.CurrentBoard.Board[cell.Row, cell.Col] == 0) {
                    if (cell.Moves.Count > 1) {
                        cell.Moves = cell.Moves.OrderByDescending(move => move.MoveScore.ScoreValue).ToList();
                    }

                    foreach (var move in cell.Moves) {
                        movesPlayed.Add(move);
                        var newmv = new MoveResult(new BoardCells(new Board(board.CurrentBoard.Board, move)), movesPlayed, (List<IMove>)null);

                        _numMovesTried++;
                        var newresult = SolveBoard(newmv);
                        if (newresult != null && IsSolved(newresult)) {
                            return newresult;
                        }

                        movesPlayed.Remove(move);
                    }
                }
            }

            if (IsSolved(board)) {
                return board;
            }

            return null;
        }

        protected internal bool IsSolved(MoveResult moveResult) {
            var board = moveResult.CurrentBoard.Board;
            // search for a zero value and return
            for (int row = 0; row < board.Size; row++) {
                for (int col = 0; col < board.Size; col++) {
                    if (board[row, col] == 0) {
                        return false;
                    }
                }
            }

            if (!Board.IsValid((Board)board)) {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Will play all forced moves. The result returned will be the resulting
        /// board and an unsorted move list which does not have any forced moves.
        /// </summary>
        protected MoveResult PlayForcedMoves(IBoardCells board, List<IMove> moves) {
            IBoard playboard = board.Board;
            List<IMove> movesPlayed = new List<IMove>();
            List<IMove> forcedMoves = GetForcedMoves(moves);
            IBoardCells playboardCells = new BoardCells(playboard);

            List<Cell> movesRemaining = null;
            do {
                foreach (var move in forcedMoves) {
                    playboard = new Board(playboard, move);
                    movesPlayed.Add(move);
                    _numMovesTried++;
                }

                playboardCells = new BoardCells(playboard);
                movesRemaining = MoveFinder.FindMoves(playboardCells);
                forcedMoves = GetForcedMoves(Board.GetMovesFrom(movesRemaining));
            } while (forcedMoves.Count > 0);

            if (!Board.IsValid((Board)playboard)) {
                return null;
            }

            return new MoveResult(playboardCells, movesPlayed, movesRemaining);
        }

        protected List<IMove> GetForcedMoves(IList<IMove> moves) {
            List<IMove> forcedMoves = new List<IMove>();
            foreach (var move in moves) {
                if (move.IsForcedMove.HasValue && move.IsForcedMove.Value) {
                    forcedMoves.Add(move);
                }
            }

            return forcedMoves;
        }
    }
}
