using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services;

public interface IServiceManager
{
    IQTITestService QTITest { get; }
    IQTITestAdminService QTITestAdmin { get; }
    Task SaveAsync();
}
