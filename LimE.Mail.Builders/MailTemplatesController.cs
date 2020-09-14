namespace LimE.Mail.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Hosting;

    public class MailTemplatesController : Controller
    {
        private readonly IWebHostEnvironment env;

        private readonly ViewTemplateRenderer viewRenderer;

        public MailTemplatesController(IWebHostEnvironment env, ViewTemplateRenderer viewRenderer)
        {
            this.env = env;
            this.viewRenderer = viewRenderer;
        }

        [HttpGet]
        public async Task<IActionResult> ViewTest(string viewTemplatePath, string format = "html")
        {
            if (!this.env.IsProduction())
            {
                var fmt = Enum.Parse<ViewRendererTargetFormat>(format, true);
                return this.Content(
                    await this.viewRenderer.RenderViewAsync(
                        viewTemplatePath,
                        this.Request.Query.Where(p => p.Key != "viewTemplatePath").ToDictionary(p => p.Key, p => p.Value).AsViewData(),
                        fmt),
                    fmt == ViewRendererTargetFormat.Html ? "text/html" : "text/plain");
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}