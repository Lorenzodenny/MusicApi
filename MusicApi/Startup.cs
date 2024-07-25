using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.Repositories;
using MusicApi.Service;
using MusicApi.Service.Facade;
using MusicApi.Utilities.Decorator;
using MusicApi.Utilities.Proxies;
using MusicApi.Validators;
using MusicApi;
using FluentValidation.AspNetCore;
using MusicApi.Service.HTTP_Client;
using MusicApi.Utilities.Observers;
using MusicApi.Utilities.Commands;
using FluentValidation;
using MusicApi.DTO.RequestDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace MusicApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
           
            // Registrazione dei controller e dei servizi essenziali
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configurazione del contesto del database con Entity Framework
            services.AddDbContext<MusicApiContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Registrazione del servizio delle canzoni, album e artisti con iniezione di dipendenza
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();

            // Registrazione Repository generico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Aggiungi AutoMapper
            services.AddAutoMapper(typeof(ArtistMappingProfile), typeof(AlbumMappingProfile), typeof(SongMappingProfile));

            // Registrazione dei controller e FluentValidation
            services.AddControllers();

            // Registrazione esplicita dei validatori
            services.AddTransient<IValidator<CreateAlbumDTO>, CreateAlbumDTOValidator>();
            services.AddTransient<IValidator<CreateArtistDTO>, CreateArtistDTOValidator>();
            services.AddTransient<IValidator<CreateSongDTO>, CreateSongDTOValidator>();

            // Registrazione delle operazioni base e dei decorator
            services.AddScoped<ISongOperation, BasicSongOperation>();
            services.Decorate<ISongOperation, LoggingSongOperationDecorator>();

            // Aggiungi Memory Cache
            services.AddMemoryCache();

            // Registrazione del Proxy per il caching
            services.Decorate<ISongService, CachingSongServiceProxy>();

            // Registrazione Facade
            services.AddScoped<IMusicManagementFacade, MusicManagementFacade>();

            // Registra HttpClient
            services.AddHttpClient();

            // Configura Client
            services.AddHttpClient<FakeApiService>();

            // Registrazione del Command Invoker e del suo esecutore concreto
            services.AddScoped<ICommandInvoker, CommandInvoker>();

            // ** Aggiungi la registrazione di ISubject e i suoi componenti correlati **
            services.AddSingleton<ISubject, Subject>();
            services.AddSingleton<IObserver, LoggerObserver>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configurazione del middleware, inclusa la documentazione Swagger in sviluppo
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware per l'autorizzazione e il routing delle richieste
            app.UseRouting();
            app.UseAuthorization();  // Abilita l'autorizzazione
            // Mappatura dei controller
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

