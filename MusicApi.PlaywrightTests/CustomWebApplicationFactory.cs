using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicApi;
using MusicApi.DataAccessLayer;
using MusicApi.Model;
using System.Linq;

namespace MusicApi.PlaywrightTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Rimuovi il contesto del database esistente
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MusicApiContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Aggiungi un contesto del database in memoria
                services.AddDbContext<MusicApiContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Crea un provider per risolvere i servizi
                var serviceProvider = services.BuildServiceProvider();

                // Crea un ambito per risolvere il contesto del database
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MusicApiContext>();
                db.Database.EnsureCreated();

                // Seed del database per i test
                SeedDatabase(db);
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();
            host.Start();
            return host;
        }

        private void SeedDatabase(MusicApiContext context)
        {
            // Aggiungi qui i dati di test al contesto del database
            context.Songs.AddRange(
                new Song { Name = "Song 1", Year = 2020, AlbumId = 1 },
                new Song { Name = "Song 2", Year = 2021, AlbumId = 1 }
            );
            context.SaveChanges();
        }
    }
}
