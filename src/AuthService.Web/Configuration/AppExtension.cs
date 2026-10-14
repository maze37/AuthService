using Framework.Middlewares;
using Serilog;

namespace AuthService.Web.Configuration;

public static class AppExtension
{
    public static WebApplication ConfigureExtensions(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseExceptionMiddleware();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.MapHealthChecks("/health");

        return app;
    }
}
