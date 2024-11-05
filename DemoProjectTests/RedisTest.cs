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
    private ConfigurationOptions _options => new RedisOptionsFactory().Options;

    [Fact]
    public async void Connect_with_endpoint()
    {
        IDatabase db = ConnectionMultiplexer.Connect(_options).GetDatabase();
        
        double timeToPing = (await db.PingAsync()).TotalMilliseconds;

        Assert.True(timeToPing < 100);
    }

    [Fact]
    public async void Contain_qtiTest_index()
    {
        var provider = new RedisConnectionProvider(_options);
        
        RedisIndexInfo? info = await provider.Connection.GetIndexInfoAsync(typeof(QTITest));

        Assert.NotNull(info);
    }
}
