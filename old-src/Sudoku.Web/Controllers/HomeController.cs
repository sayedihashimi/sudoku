using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Text;

namespace Sudoku.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPlayer Player { get; }

        public HomeController() {
            var finder = new SimpleMoveFinder();
            Player = new SimplePlayer(finder, new SimpleEvaluator(finder));
        }

        public IActionResult Index()
        {
            IBoard board = new Board("8..3......9.....7...3584...4...9...2...4.7...78...1..5..96...2.....5..31........6");
            return View(board);
        }
        
        [HttpPost]
        public IActionResult Solve(string newboard) {
            if(newboard != null) {
                newboard = newboard.Replace(".", "0");
            }
            // make sure that the string only contains numbers by converting to an int
            double theint;
            if (!double.TryParse(newboard,out theint)) {
                return View("Index");
            }

            try {
                var result = Player.SolveBoard(new Board(newboard));
                if(result != null) {
                    return View("Solved", result);
                    // return View("Index", result.CurrentBoard.Board);
                }
            }
            catch(Exception ex) {
                return View("Index");
            }

            return View("Index", new Board(newboard));
        }

        [HttpPost]
        public IActionResult SolveFromGrid(int[] board) {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < board.Length; i++) {
                sb.Append(board[i].ToString());
            }

            return Solve(sb.ToString());
        }
    }
}
