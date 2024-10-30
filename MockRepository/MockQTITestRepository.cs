//using Contracts.Repositories;
//using Entities.Models;
//using Entities.Enums;
//using System.Linq.Expressions;


//namespace Repository;

//public class MockQTITestRepository : IQTITestRepository, IRepositoryBase<QTITest>
//{
//    protected List<QTITest> Data = MockQTITestRepositoryConfiguration.InitialData();

//    public void Create(QTITest entity)
//    {
//        entity.Id = Guid.NewGuid();
//        Data.Add(entity);
//    }

//    public Task<QTITest> CreateQTITest(QTITest qtiTest)
//    {
//        qtiTest.Id = Guid.NewGuid();
//        Data.Add(qtiTest);
//        return Task.FromResult(qtiTest);
//    }

//    public void Delete(QTITest entity)
//    {
//        Data.Remove(Data.FirstOrDefault(t => t.Id == entity.Id)!);
//    }

//    public Task DeleteQTITest(QTITest qtiTest)
//    {
//        Data.Remove(qtiTest);
//        return Task.CompletedTask;
//    }

//    public IQueryable<QTITest> FindAll(bool trackChanges)
//    {
//        return Data.AsQueryable();
//    }

//    public IQueryable<QTITest> FindByCondition(Expression<Func<QTITest, bool>> condition, bool trackChanges)
//    {
//        return  Data.AsQueryable().Where(condition);
//    }
    
//    public Task<QTITest> GetTestByCondition(Func<QTITest, bool> condition)
//    {
//        return Task.FromResult(Data.First(condition)!);
//    }

//    public Task<IEnumerable<QTITest>> GetAllQTITests()
//    {
//        return Task.FromResult(Data.AsEnumerable());
//    }

//    public Task<QTITest> GetQTITestById(Guid id)
//    {
//        return Task.FromResult(Data.FirstOrDefault(t => t.Id == id)!);
//    }

//    public void Update(QTITest entity)
//    {
//        Data.Remove(Data.FirstOrDefault(t => t.Id == entity.Id)!);
//        Data.Add(entity);
//    }

//    public Task<QTITest> UpdateQTITest(QTITest qtiTest)
//    {
//        Data.Remove(Data.FirstOrDefault(t => t.Id == qtiTest.Id)!);
//        Data.Add(qtiTest);
//        return Task.FromResult(qtiTest);
//    }

//    public Task<IEnumerable<QTITest>> GetTestsByCondition(Func<QTITest, bool> condition)
//    {
//        return Task.FromResult(Data.Where(condition)!);
//    }
//}