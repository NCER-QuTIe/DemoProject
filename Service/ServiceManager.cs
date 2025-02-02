using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, ICreateQTIProcessorService converterService) : IServiceManager
{
    private Lazy<IFeedbackService> _feedbackSerice => new Lazy<IFeedbackService>(() => new FeedbackService(repositoryManager, mapper, loggerManager));
    private Lazy<IQTITestService> _qtiTest => new Lazy<IQTITestService>(() => new QTITestService(repositoryManager, loggerManager, mapper));
    private Lazy<IQTITestAdminService> _qtiTestAdmin => new Lazy<IQTITestAdminService>(() => new QTITestAdminService(repositoryManager, loggerManager, mapper, converterService));

    public Task SaveAsync() => Task.CompletedTask;

    public IQTITestService QTITest => _qtiTest.Value;
    public IQTITestAdminService QTITestAdmin => _qtiTestAdmin.Value;
    public IFeedbackService Feedback => _feedbackSerice.Value;
}