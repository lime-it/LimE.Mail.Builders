namespace Test.LimE.Mail.Builders
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Hosting;

    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T>
        where T : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var c = webBuilder.UseStartup<T>();
                });
            return builder;
        }
    }
}