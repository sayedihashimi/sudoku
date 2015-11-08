namespace Sudoku.Web {
    using Microsoft.AspNet.Razor.Runtime.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MoveResultTagHelper : SudokuBaseTagHelper {

        public MoveResultTagHelper() {
            // this.DisplaySteps = true;
        }

        [HtmlAttributeName("MoveResult")]
        public MoveResult MoveResult { get; set; }

        //[HtmlAttributeName("DisplaySteps")]
        //public bool DisplaySteps { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            // [initial-board] -> [solved-board]
            // seqences of boards

            if(MoveResult == null) {
                output.SuppressOutput();
                return;
            }
            output.TagName = "span";
            AddBoardToOutput(MoveResult.OriginalBoard.Board, output);

            output.Content.AppendEncoded(Environment.NewLine);
            output.Content.AppendEncoded(@"&nbsp;");

            AddBoardToOutput(MoveResult.CurrentBoard.Board, output);

            //if (DisplaySteps) {
            //    output.Content.AppendEncoded(Environment.NewLine);
            //    output.Content.AppendEncoded(@"<br/>");
            //    output.Content.AppendEncoded(@"<div style=""height: 10px;"">&nbsp;</div>");

            //    foreach (var move in MoveResult.MovesPlayed) {
            //        AddBoardToOutput(move.Board, output);
            //        output.Content.AppendEncoded(@"&nbsp;");
            //        output.Content.AppendEncoded(Environment.NewLine);
            //    }
            //}
        }
    }
}
