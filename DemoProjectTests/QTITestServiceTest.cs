using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using Entities.Models;
using LoggerService;
using Redis.OM;
using Redis.OM.Contracts;
using Repository;
using Service;
using StackExchange.Redis;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Exceptions;
using Entities.Models.Configurations;

namespace DemoProjectTests;

public class QTITestServiceTest
{
    private IServiceManager _serviceManager;
    private IRepositoryManager _reppositoryManager;
    private IRedisConnectionProvider _provider;
    private ILoggerManager _loggerManager;
    private IMapper _mapper;

    private ConfigurationOptions Options => new RedisOptionsFactory().Options;

    public QTITestServiceTest()
    {
        _provider = new RedisConnectionProvider(Options);
        _reppositoryManager = new RepositoryManager(_provider);
        _loggerManager = new LoggerManager();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _serviceManager = new ServiceManager(_reppositoryManager, _loggerManager, _mapper);

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
        IQTITestService service = _serviceManager.QTITest;

        Assert.NotNull(service);
    }

    [Fact]
    public async void Get_qtiTest()
    {
        IQTITestService service = _serviceManager.QTITest;
        QTITest test = InsertRandomTest();

        QTITestDTO testDTO = await service.GetQTITestById(test.Id);

        Assert.NotNull(testDTO);
        Assert.Equal(testDTO.Id, test.Id);
        Assert.Equal(testDTO.Name, test.Name);

        DisposeTest(test);
    }

    [Fact]
    public async void Get_qtiTest_with_invalid_id()
    {
        IQTITestService service = _serviceManager.QTITest;
        QTITest test = GenerateTest();

        await Assert.ThrowsAsync<QTITestByIdNotFoundException>(async () => await service.GetQTITestById(test.Id));
    }

    [Fact]
    public async void Get_qtiTest_with_name()
    {
        IQTITestService service = _serviceManager.QTITest;
        QTITest test = InsertRandomTest();

        QTITestDTO testDTO = await service.GetQTITestByName(test.Name!);

        Assert.NotNull(testDTO);

        DisposeTest(test);
    }

    [Fact]
    public async void Get_qtiTest_with_invalid_name()
    {
        IQTITestService service = _serviceManager.QTITest;
        QTITest test = GenerateTest();

        await Assert.ThrowsAsync<QTITestByNameNotFoundException>(async () => await service.GetQTITestByName(test.Name!));
    }

    [Fact]
    public async void Get_qtiTests()
    {
        IQTITestService service = _serviceManager.QTITest;
        QTITest test1 = InsertRandomTest();
        QTITest test2 = InsertRandomTest();

        IEnumerable<QTITestDTO> tests = await service.GetQTITests();

        Assert.Single(tests.Where(dto => dto.Id == test1.Id));
        Assert.Single(tests.Where(dto => dto.Id == test2.Id));

        DisposeTest(test1);
        DisposeTest(test2);
    }

    private QTITest InsertRandomTest()
    {
        QTITest test = GenerateTest();
        InsertTest(test).Wait();
        return test;
    }

    private QTITest GenerateTest()
    {
        Random rand = new Random();
        return new QTITest { Name = $"Test_QTITestServiceTest_Test_{rand.NextInt64()}", Description = "desc", PackageBase64 = "asdasd", Status = Entities.Enums.TestStatusEnum.Active };
    }

    private async Task InsertTest(QTITest test)
    {
        await _reppositoryManager.QTITest.CreateQTITestAsync(test);
    }

    private async void DisposeTest(QTITest test)
    {
        await _reppositoryManager.QTITest.DeleteQTITestAsync(test);
    }
}
