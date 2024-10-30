using Entities.Exceptions;
using Entities.Models;
using Redis.OM;
using StackExchange.Redis;
using InitRedis;

var provider = new RedisConnectionProvider("redis://localhost:6379");
provider.Connection.DropIndexAndAssociatedRecords(typeof(QTITest));
await provider.Connection.CreateIndexAsync(typeof(QTITest));

var opts = new ConfigurationOptions()
{
    EndPoints = { "localhost:6379" }
};

var qtiTests = provider.RedisCollection<QTITest>();

await qtiTests.InsertAsync(QTITestConfiguration.InitialData());
await qtiTests.SaveAsync();

Guid dd = Guid.Empty;
var testToDelete = qtiTests.Where(t => t.Id == dd);
qtiTests.Delete(testToDelete);
qtiTests.Save();

foreach (var item in qtiTests)
{
    Console.WriteLine(item.Name + "----> " + item.Id);
}

//var chck = await provider.Connection.IsIndexCurrentAsync(typeof(QTITest));
//Console.WriteLine(chck);
