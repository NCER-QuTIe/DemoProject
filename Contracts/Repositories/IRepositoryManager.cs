﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories;

public interface IRepositoryManager
{
    IQTITestRepository QTITest{ get; }
    IExternalTestRepository ExternalTest { get;  }
    IFeedbackRepository Feedback { get; }
    Task SaveAsync();
}