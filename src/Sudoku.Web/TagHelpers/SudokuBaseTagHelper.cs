namespace Sudoku.Web {
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class SudokuBaseTagHelper : TagHelper {
        protected void AddBoardToOutput(IBoard board, TagHelperOutput output) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            if (output == null) { throw new ArgumentNullException(nameof(output)); }

            output.Content.AppendEncoded(@"<table>");
            for (int row = 0; row < board.Size; row++) {
                output.Content.AppendEncoded(@"<tr>");
                for (int col = 0; col < board.Size; col++) {
                    output.Content.AppendEncoded($"<td>{board[row, col]}</td>");
                }
                output.Content.AppendEncoded(@"</tr>");
            }
            output.Content.AppendEncoded(@"</table>");
        }
    }
}
