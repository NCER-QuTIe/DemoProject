using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Repositories;

public interface IQTITestRepository 
{
    public Task<List<QTITest>> GetAllQTITestsAsync();
    public Task<List<QTITest>> GetQTITestsByConditionAsync(Expression<Func<QTITest, bool> > condition);
    public Task<QTITest?> GetQTITestByIdAsync(Guid id);
    public Task<QTITest> CreateQTITestAsync(QTITest qtiTest);
    public Task<QTITest> UpdateQTITestAsync(QTITest qtiTest);
    public Task DeleteQTITestAsync(QTITest qtiTest);
}
