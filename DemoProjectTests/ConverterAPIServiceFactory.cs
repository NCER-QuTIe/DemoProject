using Contracts.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectTests;

class ConverterAPIServiceFactory
{
    public readonly ICreateQTIProcessorService ConverterAPIService;

    public ConverterAPIServiceFactory()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

        var configs = builder.Build();
        //TODO: fix this mess ;-;. This is a temporary solution to get the testing working.
        ConverterAPIService = new CreateQTIProcessorService(configs, null, null);
    }
}
