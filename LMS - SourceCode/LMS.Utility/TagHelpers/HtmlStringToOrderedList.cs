using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.TagHelpers
{
    [HtmlTargetElement("StringToOrderedList")]
    public class HtmlStringToOrderedList:TagHelper
    {
        public string HtmlContent{ get; set; }
        public string CssClasses { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("class", this.CssClasses);
            var sb = new StringBuilder();
            sb.AppendFormat("<ol>{0}</ol>", this.HtmlContent);
            output.PreContent.SetHtmlContent(sb.ToString());
        }
    }
}
