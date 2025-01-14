using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Enums;
using Entities.Exceptions;
using Entities.Models;
using Entities.Models.Configurations;
using LoggerService;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Redis.OM.Contracts;
using Repository;
using Service;
using StackExchange.Redis;

namespace DemoProjectTests;

public class QTITestAdminServiceTest
{
    private IServiceManager _serviceManager;
    private IRepositoryManager _reppositoryManager;
    private IRedisConnectionProvider _provider;
    private ILoggerManager _loggerManager;
    private IMapper _mapper;
    private readonly ICreateQTIProcessorService _converterService = new ConverterAPIServiceFactory().ConverterAPIService;

    private ConfigurationOptions Options => new RedisOptionsFactory().Options;

    public QTITestAdminServiceTest()
    {
        _provider = new RedisConnectionProvider(Options);
        _reppositoryManager = new RepositoryManager(_provider);
        _loggerManager = new LoggerManager();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _serviceManager = new ServiceManager(_reppositoryManager, _loggerManager, _mapper, _converterService);

        if(_provider.Connection.GetIndexInfo(typeof(QTITest)) == null)
        {
            _provider.Connection.DropIndex(typeof(QTITest));
            _provider.Connection.CreateIndex(typeof(QTITest));
            _provider.RedisCollection<QTITest>().InsertAsync(QTITestConfiguration.InitialData()).Wait();
        }
    }

    [Fact]
    public void Exists()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;

        Assert.NotNull(service);
    }

    [Fact]
    public async void Get_qtiTest()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
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
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = GenerateTest();

        await Assert.ThrowsAsync<QTITestByIdNotFoundException>(async () => await service.GetQTITestById(test.Id));
    }

    [Fact]
    public async void Get_qtiTest_with_name()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = InsertRandomTest();

        QTITestDTO testDTO = await service.GetQTITestByName(test.Name!);

        Assert.NotNull(testDTO);

        DisposeTest(test);
    }

    [Fact]
    public async void Get_qtiTest_with_invalid_name()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = GenerateTest();

        await Assert.ThrowsAsync<QTITestByNameNotFoundException>(async () => await service.GetQTITestByName(test.Name!));
    }

    //[Fact]
    //public async void Patch_qtiTest_status()
    //{
    //    IQTITestAdminService service = _serviceManager.QTITestAdmin;
    //    QTITest test = GenerateTest();
    //    test.Status = TestStatusEnum.InActive;
    //    InsertTest(test);

    //    await service.PatchQTITestStatus(test.Id, TestStatusEnum.Active);
    //    QTITestDTO testDTO = await service.GetQTITestById(test.Id);

    //    Assert.Equal(TestStatusEnum.Active, testDTO.Status);

    //    DisposeTest(test);
    //}

    //[Fact]
    //public async void Patch_invalid_qtiTest_status()
    //{
    //    IQTITestAdminService service = _serviceManager.QTITestAdmin;
    //    QTITest test = GenerateTest();
         
    //    await Assert.ThrowsAsync<QTITestByIdNotFoundException>(async () => await service.PatchQTITestStatus(test.Id, TestStatusEnum.Active));

    //    DisposeTest(test);
    //}

    [Fact] 
    public async void Delete_qtiTest()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = InsertRandomTest();

        await service.DeleteQTITestByIdAsync(test.Id);

        QTITest? fetchedTest = await _reppositoryManager.QTITest.GetQTITestByIdAsync(test.Id);
        Assert.Null(fetchedTest);
    }

    [Fact]
    public async void Delete_invalid_qtiTest()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = GenerateTest();

        await Assert.ThrowsAsync<QTITestDeleteByIdNotFoundException>(async () => await service.DeleteQTITestByIdAsync(test.Id));
    }

    [Fact]
    public async void Create_qtiTest_with_bad_package()
    {
        IQTITestAdminService service = _serviceManager.QTITestAdmin;
        QTITest test = GenerateTest();
        test.PackageBase64 = "bad_package";
        QTITestCreationDTO testDTO = _mapper.Map<QTITest, QTITestCreationDTO>(test);

        await Assert.ThrowsAsync<ConverterAPIServiceBadRequestException>(async () => await service.CreateQTITest(testDTO));
    }


    private QTITest InsertRandomTest()
    {
        QTITest test = GenerateTest();
        InsertTest(test);
        return test;
    }

    private QTITest GenerateTest()
    {
        Random rand = new Random();
        return new QTITest { Name = $"Test_QTITestAdminServiceTest_Test_{rand.NextInt64()}", Description = "desc", PackageBase64 = "asdasd", Status = Entities.Enums.TestStatusEnum.InActive };
    }

    private async void InsertTest(QTITest test)
    {
        await _reppositoryManager.QTITest.CreateQTITestAsync(test);
    }

    private async void DisposeTest(QTITest test)
    {
        await _reppositoryManager.QTITest.DeleteQTITestAsync(test);
    }
}
