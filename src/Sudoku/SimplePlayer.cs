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
            var bc = new BoardCells(board);
            var result = SolveBoard(new MoveResult(bc, new List<IMove>(), (List<IMove>)null));

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

            List<CellMoves> boardmoves = MoveFinder.FindMoves(board.CurrentBoard);
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

                boardmoves = boardmoves.OrderBy(bm => {
                    double score = 0;
                    score = bm.Moves.Count;
                    double movescore = 0.0d;
                    foreach (var m in bm.Moves) {
                        movescore += m.MoveScore.ScoreValue;
                    }

                    movescore = (-1.0d) / movescore;

                    return score + movescore;
                }
                ).ToList();
            }

            foreach (var cell in boardmoves) {
                if (cell.Moves.Count > 0 && board.CurrentBoard.Board[cell.Row, cell.Col] == 0) {
                    if (cell.Moves.Count > 1) {
                        cell.Moves = cell.Moves.OrderBy(move => move.MoveScore.ScoreValue * (-1.0d)).ToList();
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
            else {
                return null;
            }
        }

        protected internal bool IsSolved(MoveResult moveResult) {
            if (moveResult == null) { throw new ArgumentNullException(nameof(moveResult)); }

            var board = moveResult.CurrentBoard.Board;
            // search for a zero value and return
            for (int row = 0; row < board.Size; row++) {
                for (int col = 0; col < board.Size; col++) {
                    if (board[row, col] == 0) {
                        return false;
                    }
                }
            }

            if (!Board.Validate((Board)board)) {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Will play all forced moves. The result returned will be the resulting
        /// board and an unsorted move list which does not have any forced moves.
        /// </summary>
        protected MoveResult PlayForcedMoves(IBoardCells board, List<IMove> moves) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            if (moves == null) { throw new ArgumentNullException(nameof(moves)); }

            IBoardCells playboardCells = new BoardCells(board);
            IBoard playboard = playboardCells.Board;
            List<IMove> movesPlayed = new List<IMove>();
            List<IMove> forcedMoves = GetForcedMoves(moves);
            do {
                foreach (var move in forcedMoves) {
                    playboard = new Board(playboard, move);
                    movesPlayed.Add(move);
                    _numMovesTried++;
                }

                forcedMoves = GetForcedMoves(Board.GetMovesFrom(MoveFinder.FindMoves(new BoardCells(playboard))));
            } while (forcedMoves.Count > 0);

            playboardCells = new BoardCells(playboard);

            var result = new MoveResult(playboardCells, movesPlayed, MoveFinder.FindMoves(playboardCells));

            if (!Board.Validate((Board)result.CurrentBoard.Board)) {
                return null;
            }

            return result;
        }

        protected List<IMove> GetForcedMoves(IList<IMove> moves) {
            if (moves == null) { throw new ArgumentNullException(nameof(moves)); }

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
