namespace Sudoku.Web {
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BoardTagHelper : SudokuBaseTagHelper {

        [HtmlAttributeName("board")]
        public IBoard Board { get; set; }

        [HtmlAttributeName("editable")]
        public bool Editable { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            if(Board == null) {
                Board = new Board();
            }

            output.TagName = "span";

            AddBoardToOutput(Board, output, Editable);
        }
    }
}
