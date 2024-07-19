using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.Mapping;
using MusicApi.Repositories;
using MusicApi.Service;
using MusicApi.Service.Facade;
using MusicApi.Utilities.Decorator;
using MusicApi.Utilities.Proxies;
using MusicApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Registrazione dei controller e dei servizi essenziali
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurazione del contesto del database con Entity Framework
builder.Services.AddDbContext<MusicApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrazione del servizio delle canzoni, album e artisti con iniezione di dipendenza
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();

// REgistrazione Repository generico
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Aggiungi AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Registrazione Fluent Validation
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateArtistDTOValidator>());


// Registrazione delle operazioni base e dei decorator
builder.Services.AddScoped<ISongOperation, BasicSongOperation>();
builder.Services.Decorate<ISongOperation, LoggingSongOperationDecorator>();


// Aggiungi Memory Cache
builder.Services.AddMemoryCache();

// Registrazione del Proxy per il caching
builder.Services.Decorate<ISongService, CachingSongServiceProxy>();

// Registrazione Facade
builder.Services.AddScoped<IMusicManagementFacade, MusicManagementFacade>();


var app = builder.Build();

// Configurazione del middleware, inclusa la documentazione Swagger in sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware per l'autorizzazione e il routing delle richieste
app.UseAuthorization();
app.MapControllers();

// Avvio dell'applicazione
app.Run();
