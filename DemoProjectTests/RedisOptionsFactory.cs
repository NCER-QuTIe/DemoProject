using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectTests;

class RedisOptionsFactory
{
    public ConfigurationOptions Options { get; set; }

    public RedisOptionsFactory()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

        var configs = builder.Build();
        Options = new ConfigurationOptions() { EndPoints = { configs.GetConnectionString("Redis")!} };
    }
}
