namespace Sudoku {
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        private List<string> FailedBoards { get; set; }
        public MoveResult SolveBoard(IBoard board) {
            FailedBoards = new List<string>();
            var originalBoard = new BoardCells(board);
            var result = SolveBoard(new MoveResult(originalBoard, new List<IMove>(), (List<IMove>)null));

            result.OriginalBoard = originalBoard;
            return result;
        }

        public MoveResult SolveBoard(MoveResult board) {            
            if (board == null) {
                return null;
            }
            if (FailedBoards.Contains(((Board)board.CurrentBoard.Board).ToFlatString())) {
                return null;
            }
            if (IsSolved(board)) {
                return board;
            }

            if (_numMovesTried > 50000) {
                throw new InvalidOperationException("Too many moves tried sir");
            }
            
            board = PlayForcedMoves(board.CurrentBoard, Board.GetMovesFrom(MoveFinder.FindMoves(board.CurrentBoard)), board.MovesPlayed);

            if (board == null) {
                return null;
            }

            List<IMove> movesPlayed = board.MovesPlayed;
            if (movesPlayed == null) { movesPlayed = new List<IMove>(); }

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

            List<Cell> canidatecells = new List<Cell>();
            // List<IMove> canidatemoves = new List<IMove>();
            if (rowToSolve > -1) {
                for (int col = 0; col < board.CurrentBoard.Board.Size; col++) {
                    if (board.CurrentBoard.Board[rowToSolve, col] == 0) {
                        canidatecells.Add(
                            new Cell(rowToSolve, col, MoveFinder.GetMovesForCell(board.CurrentBoard, rowToSolve, col).Moves));
                    }
                }
            }
            else if (colToSolve > -1) {
                for (int row = 0; row < board.CurrentBoard.Board.Size; row++) {
                    if (board.CurrentBoard.Board[row, colToSolve] == 0) {
                        canidatecells.Add(
                            new Cell(row, colToSolve, MoveFinder.GetMovesForCell(board.CurrentBoard, row, colToSolve).Moves));
                    }
                }
            }
            else if (sqToSolve != null) {
                int row = sqToSolve[0] * sqSize;
                int col = sqToSolve[1] * sqSize;
                for (int r = row; r < row + sqSize; r++) {
                    for (int c = col; c < col + sqSize; c++) {
                        if (board.CurrentBoard.Board[r, c] == 0) {
                            canidatecells.Add(
                            new Cell(r, c, MoveFinder.GetMovesForCell(board.CurrentBoard, r, c).Moves));
                        }
                    }
                }
            }
            else {
                throw new InvalidOperationException("should not get here");
            }

            if (canidatecells.Count > 1) {
                foreach (var cc in canidatecells) {
                    if (cc.CellScore == null) {
                        cc.CellScore = Evaluator.GetCellScore(cc);
                    }
                }

                canidatecells = canidatecells.OrderByDescending(cell => cell.CellScore).ToList();
            }

            foreach (var cell in canidatecells) {
                if (cell.Moves.Count > 1) {
                    cell.Moves = cell.Moves.OrderByDescending(move => move.MoveScore).ToList();
                }

                foreach (var move in cell.Moves) {
                    movesPlayed.Add(move);
                    var newmv = new MoveResult(new BoardCells(new Board(board.CurrentBoard.Board, move)), movesPlayed, (List<IMove>)null);
                    var foo = new IMove[1];

                    _numMovesTried++;
                    var newresult = SolveBoard(newmv);
                    if (newresult != null && IsSolved(newresult)) {
                        return newresult;
                    }

                    FailedBoards.Add(((Board)board.CurrentBoard.Board).ToFlatString());
                    movesPlayed.Remove(move);
                }
            }

            if (IsSolved(board)) {
                return board;
            }

            FailedBoards.Add(((Board)board.CurrentBoard.Board).ToFlatString());
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
        protected MoveResult PlayForcedMoves(IBoardCells board, List<IMove> moves,IList<IMove>previousMovesPlayed) {
            IBoard playboard = board.Board;
            List<IMove> movesPlayed = new List<IMove>();
            if(previousMovesPlayed != null) {
                movesPlayed.AddRange(previousMovesPlayed);
            }
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
