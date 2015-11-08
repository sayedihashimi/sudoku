namespace Sudoku.Web {
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BoardTagHelper : SudokuBaseTagHelper {

        [HtmlAttributeName("board")]
        public IBoard Board { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            if(Board == null) {
                Board = new Board();
            }

            output.TagName = "span";

            AddBoardToOutput(Board, output);
            //for(int row = 0; row < Board.Size; row++) {
            //    output.Content.AppendEncoded(@"<tr>");
            //    for(int col = 0; col < Board.Size; col++) {
            //        output.Content.AppendEncoded($"<td>{Board[row,col]}</td>");
            //    }
            //    output.Content.AppendEncoded(@"</tr>");
            //}
        }
    }
}
