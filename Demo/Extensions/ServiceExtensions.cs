using AutoMapper;
using Contracts.Logger;
using LoggerService;
using Service;
using Contracts.Repositories;
using Repository;
using Contracts.Services;
using Redis.OM;
using Redis.OM.Contracts;
using Microsoft.Extensions.Configuration;

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

    public static void ConfigureIRedisProviderService(this IServiceCollection services)
    {
        services.AddSingleton<IRedisConnectionProvider>(new RedisConnectionProvider("localhost:6379"));
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
}