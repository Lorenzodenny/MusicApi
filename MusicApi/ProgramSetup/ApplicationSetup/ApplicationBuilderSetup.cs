using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MusicApi.Abstract;

namespace MusicApi.ProgramSetup.ApplicationSetup
{
    public static class ApplicationBuilderSetup
    {
        public static void ConfigureMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Attacca l'observer al subject
            var subject = app.ApplicationServices.GetRequiredService<ISubject>();
            var loggerObserver = app.ApplicationServices.GetRequiredService<IObserver>();
            subject.Attach(loggerObserver);
        }
    }

}
