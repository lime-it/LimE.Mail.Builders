namespace LimE.Mail.Builders.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class EmailImgTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();
            output.TagName = "img";
            var style = new Dictionary<string, string>()
            {
                { "display", "block" },
                { "border", "0" }
            };
            output.Attributes.MergeInlineStyles(style);
        }
    }
}
