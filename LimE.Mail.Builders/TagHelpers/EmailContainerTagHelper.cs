namespace LimE.Mail.Builders.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LimE.Mail.Builders.TagHelpers.CssUtils;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class EmailContainerTagHelper : TagHelper
    {
        private readonly Dictionary<string, string> tableDefaultAttributes = new Dictionary<string, string>()
        {
            { "cellpadding", "0" },
            { "cellspacing", "0" }
        };

        private readonly Dictionary<string, string> tableDefaultInlineStyles = new Dictionary<string, string>()
        {
            { "border-collapse", "collapse" }
        };

        public int BorderSize { get; set; } = 0;

        public string BorderColor { get; set; }

        public int Padding { get; set; } = 0;

        public int Margin { get; set; } = 0;

        public string Width { get; set; } = "100%";

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";

            var attributes = output.Attributes.GetAttributes().Merge(this.tableDefaultAttributes)
                .Where(p => !p.Key.StartsWith(Constants.TagHelperPrefix)).ToDictionary(p => p.Key, p => p.Value);
            var styles = output.Attributes.GetInlineStyles().Merge(this.tableDefaultInlineStyles);

            if (styles.ContainsKey("background-color"))
            {
                attributes["bgcolor"] = new CssColor(styles["background-color"]);
                styles.Remove("background-color");
            }

            attributes["width"] = new CssSize(this.Width);
            attributes["cellpadding"] = this.Padding.ToString();
            attributes["cellspacing"] = this.Margin.ToString();

            if (this.BorderSize > 0)
            {
                var inner = attributes.MergeCopy();
                var outer = attributes.MergeCopy();

                inner["width"] = "100%";
                inner["style"] = styles.AsCssString();
                inner["bgcolor"] = inner.TryGet("bgcolor") ?? "#ffffff";

                outer["bgcolor"] = new CssColor(this.BorderColor ?? "#ffffff");
                outer["cellpadding"] = this.BorderSize.ToString();

                output.Attributes.SetAttributes(outer);
                output.Attributes.SetInlineStyles(this.tableDefaultInlineStyles);

                output.PreContent.SetHtmlContent("<tr><td>");
                output.PreContent.AppendHtml("<table>".AddAttributes(inner));

                output.PostContent.SetHtmlContent("</table></td></tr>");
            }
            else
            {
                output.Attributes.SetAttributes(attributes);
                output.Attributes.SetInlineStyles(styles);
            }

            return Task.CompletedTask;
        }
    }
}
