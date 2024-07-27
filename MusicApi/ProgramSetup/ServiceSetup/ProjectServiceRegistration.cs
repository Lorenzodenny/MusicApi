using Microsoft.Extensions.DependencyInjection;
using MusicApi.ProgramSetup.ServiceRegistration;

namespace MusicApi.ProgramSetup.ServiceSetup
{
    public static class ProjectServiceRegistration
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            ServicesRegistration.AddDomainServices(services, configuration);
            AuthenticationConfiguration.AddAuthenticationConfiguration(services, configuration);
            SwaggerConfiguration.AddSwaggerConfiguration(services);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }
    }
}
