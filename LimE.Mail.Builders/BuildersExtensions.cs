namespace LimE.Mail.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class BuildersExtensions
    {
        public static void AddMailTemplateBuilders(this IServiceCollection services)
        {
            services.AddMailTemplateBuilders(null);
        }

        public static void AddMailTemplateBuilders(this IServiceCollection services, Action<MailTemplateBuildersOptions> configureOptions)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ViewTemplateRenderer>();

            services.AddMvcCore().AddApplicationPart(typeof(BuildersExtensions).Assembly);

            if (configureOptions != null)
            {
                services.PostConfigureAll(configureOptions);
            }
        }

        public static ViewDataDictionary AsViewData<T>(this IEnumerable<KeyValuePair<string, T>> ext)
        {
            ViewDataDictionary tmp = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            if (ext != null)
            {
                foreach (KeyValuePair<string, T> kv in ext)
                {
                    tmp.Add(kv.Key, kv.Value);
                }
            }

            return tmp;
        }
    }
}
