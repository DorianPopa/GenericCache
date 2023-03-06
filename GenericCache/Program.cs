using GenericCachePoC.Caching;

namespace GenericCachePoC.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.SetupDependencyInjectionContainer(configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

internal static class ServiceExtensions
{
    public static IServiceCollection SetupDependencyInjectionContainer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheLayer(configuration)
                .AddApiPresentationLayer();

        return services;
    }
}
