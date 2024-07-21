using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace MusicApi.Tests.TestWebApplicationFactory
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    var projectDir = Directory.GetCurrentDirectory();
                    var solutionDir = Directory.GetParent(projectDir)?.Parent?.Parent?.Parent?.FullName;
                    var webProjectDir = Path.Combine(solutionDir ?? string.Empty, "MusicApi");
                    webBuilder.UseContentRoot(webProjectDir);

                    // Qui puoi sovrascrivere o modificare i servizi per i test
                    // Ad esempio, utilizzare un database in-memory per i test:
                    // webBuilder.ConfigureServices(services =>
                    // {
                    //     services.AddDbContext<MusicApiContext>(options =>
                    //         options.UseInMemoryDatabase("TestDb"));
                    // });
                });
        }
    }
}
