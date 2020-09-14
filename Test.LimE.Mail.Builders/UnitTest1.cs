namespace Test.LimE.Mail.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::LimE.Mail.Builders;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public UnitTest1(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Test()
        {
            var values = new Dictionary<string, string>()
            {
                { "name", "test" }
            };

            var viewRenderer = this.factory.Services.GetRequiredService<ViewTemplateRenderer>();

            var result = await viewRenderer.RenderViewAsync("Example-0", values.AsViewData(), ViewRendererTargetFormat.Html);

            Assert.Contains("Hello", result);
            Assert.Contains("test", result);
        }
    }
}
