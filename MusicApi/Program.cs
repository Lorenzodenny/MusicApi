using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicApi;
using MusicApi.Abstract;
using MusicApi.DataAccessLayer;
using MusicApi.DTO.RequestDTO;
using MusicApi.Repositories;
using MusicApi.Service;
using MusicApi.Service.Facade;
using MusicApi.Service.HTTP_Client;
using MusicApi.Utilities.Commands;
using MusicApi.Utilities.Decorator;
using MusicApi.Utilities.Observers;
using MusicApi.Utilities.Proxies;
using MusicApi.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura i servizi usando la classe Startup
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

// Registra JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings"); // Centralizzato in appsetting.json
var secretKey = jwtSettings["SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);


// Registra l'Autenticazione
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Registra Sagger con la possibilità di ricevere un JWT per l'autenticazione
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Music API", Version = "v1" });

    // Configurazione per JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// Configurazione del contesto del database con Entity Framework
builder.Services.AddDbContext<MusicApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrazione del servizio delle canzoni, album e artisti con iniezione di dipendenza
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();

// Registrazione Repository generico
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Registra tutte le classi di mapping da un assembly specifico, se sono tutte nello stesso assembly
//builder.Services.AddAutoMapper(typeof(Startup).Assembly); => Registrazione totale

// Aggiungi AutoMapper
builder.Services.AddAutoMapper(typeof(ArtistMappingProfile), typeof(AlbumMappingProfile), typeof(SongMappingProfile));

// Registrazione Fluent Validation
builder.Services.AddTransient<IValidator<CreateAlbumDTO>, CreateAlbumDTOValidator>();
builder.Services.AddTransient<IValidator<CreateArtistDTO>, CreateArtistDTOValidator>();
builder.Services.AddTransient<IValidator<CreateSongDTO>, CreateSongDTOValidator>();

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

app.UseAuthentication(); // Abilita l'autenticazione
app.UseAuthorization();  // Abilita l'autorizzazione

app.MapControllers();

// Configura l'applicazione usando la classe Startup
startup.Configure(app, app.Environment);

app.Run();
