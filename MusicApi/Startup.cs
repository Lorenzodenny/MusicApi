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
using MusicApi.DTO.RequestDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.OpenApi.Models;
using MusicApi.Service.HTTP_Client;
using MusicApi.Utilities.Commands;
using MusicApi.Utilities.Observers;

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
            // Basic controller support without views
            services.AddControllers();

            // API documentation with Swagger/OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Music API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Database context configuration
            services.AddDbContext<MusicApiContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Service layer and business logic
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // AutoMapper for object-object mapping
            services.AddAutoMapper(typeof(ArtistMappingProfile), typeof(AlbumMappingProfile), typeof(SongMappingProfile));

            // Fluent Validation for DTOs
            services.AddTransient<IValidator<CreateAlbumDTO>, CreateAlbumDTOValidator>();
            services.AddTransient<IValidator<CreateArtistDTO>, CreateArtistDTOValidator>();
            services.AddTransient<IValidator<CreateSongDTO>, CreateSongDTOValidator>();

            // Caching and decoration
            services.AddMemoryCache();
            services.AddScoped<ISongOperation, BasicSongOperation>();
            services.Decorate<ISongOperation, LoggingSongOperationDecorator>();
            services.Decorate<ISongService, CachingSongServiceProxy>();

            // Facade for managing complex interactions
            services.AddScoped<IMusicManagementFacade, MusicManagementFacade>();

            // External HTTP calls setup
            services.AddHttpClient<FakeApiService>();
            services.AddHttpClient();

            // Command pattern and observer pattern for operations and logging
            services.AddSingleton<ISubject, Subject>();
            services.AddSingleton<IObserver, LoggerObserver>();
            services.AddScoped<ICommandInvoker, CommandInvoker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
