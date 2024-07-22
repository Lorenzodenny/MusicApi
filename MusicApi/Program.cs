using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MusicApi;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.Mapping;
using MusicApi.Repositories;
using MusicApi.Service;
using MusicApi.Service.Facade;
using MusicApi.Service.HTTP_Client;
using MusicApi.Utilities.Commands;
using MusicApi.Utilities.Decorator;
using MusicApi.Utilities.Observers;
using MusicApi.Utilities.Proxies;
using MusicApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configura i servizi usando la classe Startup
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);


// Add services to the container.
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

// Registrazione Repository generico
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

// Registra HttpClient
builder.Services.AddHttpClient();

// Configura Client
builder.Services.AddHttpClient<FakeApiService>();

// Registrazione Observer Pattern
builder.Services.AddSingleton<ISubject, Subject>();
builder.Services.AddSingleton<IObserver, LoggerObserver>();

// Registrazione Command
builder.Services.AddScoped<ICommandInvoker, CommandInvoker>();

// Registro Command di prova
// Registrazione del ProvaService
builder.Services.AddScoped<ProvaService>();

var app = builder.Build();

// Attacca l'observer al subject
var subject = app.Services.GetRequiredService<ISubject>();
var loggerObserver = app.Services.GetRequiredService<IObserver>();
subject.Attach(loggerObserver);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Configura l'applicazione usando la classe Startup
startup.Configure(app, app.Environment);

app.Run();
