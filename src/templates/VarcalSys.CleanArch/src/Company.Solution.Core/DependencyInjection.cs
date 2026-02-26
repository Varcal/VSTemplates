using Microsoft.Extensions.DependencyInjection;
using Company.Solution.Core.Services;

namespace Company.Solution.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
        return services;
    }
}
