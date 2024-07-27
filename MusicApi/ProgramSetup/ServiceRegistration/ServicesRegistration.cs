using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MusicApi.DataAccessLayer;
using MusicApi.Service;
using MusicApi.Service.Facade;
using MusicApi.DTO.RequestDTO;
using MusicApi.Repositories;
using MusicApi.Utilities.Commands;
using MusicApi.Utilities.Decorator;
using MusicApi.Utilities.Observers;
using MusicApi.Utilities.Proxies;
using MusicApi.Validators;
using FluentValidation;
using AutoMapper;
using MusicApi.Abstract;
using MusicApi.Service.HTTP_Client;
using FluentValidation.AspNetCore;

namespace MusicApi.ProgramSetup.ServiceRegistration
{
    public static class ServicesRegistration
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurazione del contesto del database con Entity Framework
            services.AddDbContext<MusicApiContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Registrazione del servizio delle canzoni, album e artisti con iniezione di dipendenza
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();

            // Registrazione Repository generico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registra tutte le classi di mapping da un assembly specifico, se sono tutte nello stesso assembly
            //builder.Services.AddAutoMapper(typeof(Startup).Assembly); => Registrazione totale

            // Aggiungi AutoMapper
            services.AddAutoMapper(typeof(ArtistMappingProfile), typeof(AlbumMappingProfile), typeof(SongMappingProfile));


            // Registrazione Fluent Validation
            services.AddControllers()
                    .AddFluentValidation(fv =>
                                fv.RegisterValidatorsFromAssemblyContaining<CreateArtistDTOValidator>());


            //// Registrazione Fluent Validation
            //services.AddTransient<IValidator<CreateAlbumDTO>, CreateAlbumDTOValidator>();
            //services.AddTransient<IValidator<CreateArtistDTO>, CreateArtistDTOValidator>();
            //services.AddTransient<IValidator<CreateSongDTO>, CreateSongDTOValidator>();

            // Registrazione delle operazioni base e dei decorator
            services.AddScoped<ISongOperation, BasicSongOperation>();
            services.Decorate<ISongOperation, LoggingSongOperationDecorator>();

            // Aggiungi Memory Cache
            services.AddMemoryCache();

            // Registrazione del Proxy per il caching
            services.Decorate<ISongService, CachingSongServiceProxy>();

            // Registrazione Facade
            services.AddScoped<IMusicManagementFacade, MusicManagementFacade>();

            // Configura Client
            services.AddHttpClient<FakeApiService>();

            // Registra HttpClient
            services.AddHttpClient();

            // Registrazione Observer Pattern
            services.AddSingleton<ISubject, Subject>();
            services.AddSingleton<IObserver, LoggerObserver>();

            // Registrazione Command
            services.AddScoped<ICommandInvoker, CommandInvoker>();

            // Registro Command di prova
            // Registrazione del ProvaService
            services.AddScoped<ProvaService>();
        }
    }
}
