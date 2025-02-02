using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories;

public interface IFeedbackRepository
{
    public Task<List<Feedback>> GetAllFeedbacksAsync();
    public Task<Feedback> GetFeedbackByIdAsync(Guid id);
    public Task<Feedback> CreateFeedbackAsync(Feedback feedback);
    public Task DeleteQTITestAsync(Feedback qtiTest);
}
