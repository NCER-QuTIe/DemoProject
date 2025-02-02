using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using Entities.Models;
using Entities.Models.Configurations;
using LoggerService;
using Redis.OM;
using Redis.OM.Contracts;
using Repository;
using Service;
using StackExchange.Redis;

namespace Demo.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        //TODO add restriction of ip address for some methods.
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }

    public static void ConfigureExternalAPIs(this IServiceCollection services)
    {
        services.AddScoped<ICreateQTIProcessorService, CreateQTIProcessorService>();
    }

    public static void ConfigureIRedisProviderService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var opts = new ConfigurationOptions()
        {
            EndPoints = { redisConnectionString! }
        };

        services.AddSingleton<IRedisConnectionProvider>(sp => new RedisConnectionProvider(opts));
        ConfigureRedisIndices(configuration);
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddSingleton<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    private static async void ConfigureQTITest(IRedisConnectionProvider provider)
    {
        if (provider.Connection.GetIndexInfo(typeof(QTITest)) == null)
        {
            provider.Connection.DropIndex(typeof(QTITest));
            provider.Connection.CreateIndex(typeof(QTITest));
            await provider.RedisCollection<QTITest>().InsertAsync(QTITestConfiguration.InitialData());
        }
    }
    
    private static async void ConfigureFeedback(IRedisConnectionProvider provider)
    {
        if (provider.Connection.GetIndexInfo(typeof(Feedback)) == null)
        {
            provider.Connection.DropIndex(typeof(Feedback));
            provider.Connection.CreateIndex(typeof(Feedback));
            await provider.RedisCollection<Feedback>().InsertAsync(FeedbackConfiguration.InitialData());
        }
    }

    private static void ConfigureRedisIndices(IConfiguration configuration)
    {
        var opts = new ConfigurationOptions()
        {
            EndPoints = { configuration.GetConnectionString("Redis")! }
        };
        var provider = new RedisConnectionProvider(opts);

        ConfigureQTITest(provider);
        ConfigureFeedback(provider);
    }
}