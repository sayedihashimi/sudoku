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
            if (IsSolved(board)) {
                return board;
            }

            List<IMove> movesPlayed = new List<IMove>();
            if (board.MovesPlayed != null && board.MovesPlayed.Count > 0) {
                movesPlayed.AddRange(board.MovesPlayed);
            }

            board = PlayForcedMoves(board.CurrentBoard, Board.GetMovesFrom(MoveFinder.FindMoves(board.CurrentBoard)));

            if (board == null) {
                return null;
            }
            if (IsSolved(board)) {
                return board;
            }

            IScore maxCurrentScore = Score.InvalidBoardScore;

            int rowToSolve = int.MinValue;
            int colToSolve = int.MinValue;

            // find the row/column/square to solve next
            for (int counter = 0; counter < board.CurrentBoard.Board.Size; counter++) {
                int[] rowcontent = board.CurrentBoard.GetRowForCell(counter, 0);
                int[] colcontent = board.CurrentBoard.GetColumnForCell(0, counter);

                IScore currentRowScore = Evaluator.GetRowScore(board.CurrentBoard, counter);
                IScore currentColScore = Evaluator.GetColScore(board.CurrentBoard, counter);

                if (currentRowScore.CompareTo(maxCurrentScore) > 0 && !Score.SolvedRegionScore.Equals(currentRowScore)) {
                    maxCurrentScore = currentRowScore;
                    rowToSolve = counter;
                    colToSolve = int.MinValue;
                }
                if (currentColScore.CompareTo(maxCurrentScore) > 0 && !Score.SolvedRegionScore.Equals(currentColScore)) {
                    maxCurrentScore = currentColScore;
                    colToSolve = counter;
                    rowToSolve = int.MinValue;
                }
            }

            int[] sqToSolve = null;
            int sqSize = (int)Math.Sqrt(board.CurrentBoard.Board.Size);
            for (int sqRow = 0; sqRow < sqSize; sqRow++) {
                for (int sqCol = 0; sqCol < sqSize; sqCol++) {
                    // board.CurrentBoard.sq
                    IScore currentSqScore = Evaluator.GetSquareScore(board.CurrentBoard, sqRow, sqCol);
                    if (currentSqScore.CompareTo(maxCurrentScore) > 0 && !Score.SolvedRegionScore.Equals(currentSqScore)) {
                        maxCurrentScore = currentSqScore;
                        sqToSolve = new int[2] { sqRow, sqCol };
                        rowToSolve = int.MinValue;
                        colToSolve = int.MinValue;
                    }
                }
            }

            List<IMove> canidatemoves = new List<IMove>();
            if(rowToSolve > -1) {
                for(int col = 0; col < board.CurrentBoard.Board.Size; col++) {
                    if (board.CurrentBoard.Board[rowToSolve, col] == 0) {
                        canidatemoves.AddRange(
                            MoveFinder.GetMovesForCell(board.CurrentBoard, rowToSolve, col).Moves);
                    }
                }
            }
            else if(colToSolve > -1){
                for(int row = 0; row < board.CurrentBoard.Board.Size; row++) {
                    if (board.CurrentBoard.Board[row, colToSolve] == 0) {
                        canidatemoves.AddRange(
                            MoveFinder.GetMovesForCell(board.CurrentBoard, row, colToSolve).Moves);
                    }
                }
            }
            else if(sqToSolve != null) {
                int row = sqToSolve[0] * sqSize;
                int col = sqToSolve[1] * sqSize;
                for(int r = row;r<row + sqSize; r++) {
                    for(int c= col;c<col+sqSize; c++) {
                        if (board.CurrentBoard.Board[r, c] == 0) {
                            canidatemoves.AddRange(
                                MoveFinder.GetMovesForCell(board.CurrentBoard, r, c).Moves);
                        }
                    }
                }
            }
            else {
                throw new InvalidOperationException("should not get here");
            }

            if(canidatemoves.Count > 1) {
                foreach(var mv in canidatemoves) {
                    if(mv.MoveScore == null) {
                        mv.MoveScore = Evaluator.GetScore(mv);
                    }
                }

                canidatemoves = canidatemoves.OrderByDescending(m => m.MoveScore).ToList();
            }

            foreach(var move in canidatemoves) {
                movesPlayed.Add(move);
                var newmv = new MoveResult(new BoardCells(new Board(board.CurrentBoard.Board, move)), movesPlayed, (List<IMove>)null);

                _numMovesTried++;
                var newresult = SolveBoard(newmv);
                if (newresult != null && IsSolved(newresult)) {
                    return newresult;
                }

                movesPlayed.Remove(move);
            }

            if (IsSolved(board)) {
                return board;
            }

            return null;
        }

        protected internal int[] GetNextRangeToSolve(IBoardCells board) {

            throw new NotImplementedException();
        }

        public MoveResult SolveBoardOld(MoveResult board) {
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

                boardmoves = boardmoves.OrderByDescending(bm => {
                    double score = 0;
                    score = -bm.Moves.Count;
                    //double movescore = 0.0d;
                    //foreach (var m in bm.Moves) {
                    //    movescore += m.MoveScore.ScoreValue;
                    //}

                    return score;
                    // return score - ((1.0d) / movescore);
                }).ToList();
            }

            foreach (var cell in boardmoves) {
                if (cell.Moves.Count > 0 && board.CurrentBoard.Board[cell.Row, cell.Col] == 0) {
                    //if (cell.Moves.Count > 1) {
                    //    cell.Moves = cell.Moves.OrderByDescending(move => move.MoveScore.ScoreValue).ToList();
                    //}

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
