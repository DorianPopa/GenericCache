namespace GenericCachePoC.Api;

/// <summary>
/// Extension methods for setting up the services from the Api presentation layer in an <see cref="IServiceCollection" />.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds services for the Api presentation layer to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> that can be used to further configure the required services.</returns>
    public static IServiceCollection AddApiPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }
}
