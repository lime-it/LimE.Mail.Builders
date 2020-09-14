namespace LimE.Mail.Builders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class ViewTemplateRenderer
    {
        public ViewTemplateRenderer(
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IHttpContextAccessor httpContextAccessor,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MailTemplateBuildersOptions> options)
        {
            this.ViewEngine = viewEngine;
            this.TempDataProvider = tempDataProvider;
            this.HttpContextAccessor = httpContextAccessor;

            this.ServiceScopeFactory = serviceScopeFactory;
            this.Options = options?.Value ?? new MailTemplateBuildersOptions();
        }

        protected IServiceScopeFactory ServiceScopeFactory { get; set; }

        protected MailTemplateBuildersOptions Options { get; set; }

        protected ICompositeViewEngine ViewEngine { get; set; }

        protected ITempDataProvider TempDataProvider { get; set; }

        protected IHttpContextAccessor HttpContextAccessor { get; set; }

        public async Task<string> RenderViewAsync(
            string path, ViewDataDictionary viewDataDictionary, ViewRendererTargetFormat format = ViewRendererTargetFormat.Html)
        {
            path = path ?? throw new ArgumentNullException(nameof(path));
            viewDataDictionary = viewDataDictionary ?? throw new ArgumentNullException(nameof(viewDataDictionary));

            viewDataDictionary.Model = null;
            viewDataDictionary[Constants.FormatAs] = format.ToString();

            path = path.EndsWith(".cshtml") ? path : $"{path}.cshtml";
            path = path.Trim('/');

            IServiceScope scope = null;
            HttpContext httpContext = this.HttpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                scope = this.ServiceScopeFactory.CreateScope();
                httpContext = new DefaultHttpContext
                {
                    RequestServices = scope.ServiceProvider
                };
            }

            ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            try
            {
                using StringWriter sw = new StringWriter();
                ViewEngineResult viewResult = this.ViewEngine.GetView(
                    $"{this.Options.ViewTemplateBasePath}/{path}", $"{this.Options.ViewTemplateBasePath}/{path}", true);

                if (viewResult?.View == null)
                {
                    throw new Exception($"View {this.Options.ViewTemplateBasePath}/{path} not found.");
                }

                ViewContext viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDataDictionary,
                    new TempDataDictionary(httpContext, this.TempDataProvider),
                    sw,
                    new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                sw.Flush();

                if (viewContext.ViewData != viewDataDictionary)
                {
                    var keys = viewContext.ViewData.Keys.ToArray();
                    foreach (var key in keys)
                    {
                        viewDataDictionary[key] = viewContext.ViewData[key];
                    }
                }

                return sw.ToString();
            }
            finally
            {
                scope?.Dispose();
            }
        }
    }
}
