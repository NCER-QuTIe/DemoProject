using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
using Entities.Models.Configurations;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Redis.OM;
using Redis.OM.Contracts;
using Repository;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

namespace DemoProjectTests;

public class QTITestRepositoryTest
{
    private ConfigurationOptions Options => new RedisOptionsFactory().Options;
    private IRedisConnectionProvider _provider => new RedisConnectionProvider(Options);
    private IRepositoryManager _repositoryManager;

    public QTITestRepositoryTest()
    {
        _repositoryManager = new RepositoryManager(_provider);

        if (_provider.Connection.GetIndexInfo(typeof(QTITest)) == null)
        {
            _provider.Connection.DropIndex(typeof(QTITest));
            _provider.Connection.CreateIndex(typeof(QTITest));
            _provider.RedisCollection<QTITest>().InsertAsync(QTITestConfiguration.InitialData()).Wait();
        }
    }

    [Fact]
    public void Exists()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;

        Assert.NotNull(repo);
    }

    [Fact]
    public async void Fetch_qtiTests()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;

        List<QTITest> tests = await repo.GetAllQTITestsAsync();

        Assert.NotEmpty(tests);
    }

    [Fact]
    public async void Create_and_fetch_qtiTest()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;
        QTITest test = GenerateTest();

        test = await repo.CreateQTITestAsync(test);
        QTITest? fetchedTest = await repo.GetQTITestByIdAsync(test.Id);

        Assert.NotNull(fetchedTest);
        Assert.Equal(test.Id, fetchedTest.Id);

        DisposeTest(test);
    }

    [Fact]
    public async void Fetch_qtiTests_with_condition()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;
        QTITest test1 = GenerateTest();
        QTITest test2 = GenerateTest();
        test2.Name = test1.Name;
        await repo.CreateQTITestAsync(test1);
        await repo.CreateQTITestAsync(test2);

        List<QTITest> allTests = await repo.GetQTITestsByConditionAsync(t => t.Name == test1.Name);

        Assert.Equal(2, allTests.Count);
     
        DisposeTest(test1);
        DisposeTest(test2);
    }

    [Fact]
    private async void Delete_qtiTest()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;
        QTITest test = GenerateTest();
        await repo.CreateQTITestAsync(test);
        
        await repo.DeleteQTITestAsync(test);

        QTITest? fetchedTest = await repo.GetQTITestByIdAsync(test.Id);
        Assert.Null(fetchedTest);
    }

    [Fact]
    private async void Update_qtiTest()
    {
        IQTITestRepository repo = _repositoryManager.QTITest;
        QTITest test = GenerateTest();
        await repo.CreateQTITestAsync(test);
        
        test.Name = "UpdatedName";
        await repo.UpdateQTITestAsync(test);

        QTITest fetchedTest = (await repo.GetQTITestByIdAsync(test.Id))!;
        Assert.Equal("UpdatedName", fetchedTest.Name);

        DisposeTest(test);
    }

    private async void DisposeTest(QTITest test)
    {
        IQTITestRepository repo = _repositoryManager.QTITest;
        await repo.DeleteQTITestAsync(test);
    }

    private QTITest GenerateTest()
    {
        Random rand = new Random();
        return new QTITest { Name = $"Test_QTIRepositoryTest_{rand.NextInt64()}", Description = "desc", PackageBase64 = "asdasd", Status = Entities.Enums.TestStatusEnum.Active };
    }
}