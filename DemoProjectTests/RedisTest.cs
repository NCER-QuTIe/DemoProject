using Entities.Models;
using Redis.OM;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProjectTests;

public class RedisTest
{
    private ConfigurationOptions _options => new ConfigurationOptions { EndPoints = { "DemoRedis_Prod:6379" } };

    [Fact]
    public void Connect_with_endpoint()
    {
        IDatabase db = ConnectionMultiplexer.Connect(_options).GetDatabase();
        
        double timeToPing = db.Ping().TotalMilliseconds;

        Assert.True(timeToPing < 100);
    }

    [Fact]
    public void Contain_qtiTest_index()
    {
        var provider = new RedisConnectionProvider(_options);
        
        RedisIndexInfo? info = provider.Connection.GetIndexInfo(typeof(QTITest));

        Assert.NotNull(info);
    }
}
