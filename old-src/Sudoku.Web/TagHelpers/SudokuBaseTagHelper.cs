namespace Sudoku.Web {
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class SudokuBaseTagHelper : TagHelper {

        public string Indent { get; set; } = "  ";
        public string NewLine { get; set; } = Environment.NewLine;

        protected void AddBoardToOutput(IBoard board, TagHelperOutput output, bool editable = false) {
            if (board == null) { throw new ArgumentNullException(nameof(board)); }
            if (output == null) { throw new ArgumentNullException(nameof(output)); }

            int counter = 0;
            output.Content.AppendEncoded($"{NewLine}{Indent}<table>{NewLine}");
            for (int row = 0; row < board.Size; row++) {

                output.Content.AppendEncoded($"{Indent}{Indent}<tr>{NewLine}");
                output.Content.AppendEncoded($"{Indent}{Indent}{Indent}");
                for (int col = 0; col < board.Size; col++) {
                    output.Content.AppendEncoded($"<td>");
                    if (!editable) {
                        output.Content.AppendEncoded($"{board[row, col]}");
                    }
                    else {
                        output.Content.AppendEncoded($"<input type=\"text\" value=\"{board[row,col]}\" name=\"board[{counter}]\" style=\"width:100%;\" maxlength=\"1\" />");
                    }
                    output.Content.AppendEncoded($"</td>");

                    counter++;
                }
                output.Content.AppendEncoded($"{NewLine}{Indent}{Indent}</tr>{NewLine}");
            }
            output.Content.AppendEncoded($"{Indent}</table>{NewLine}");
        }
    }
}
