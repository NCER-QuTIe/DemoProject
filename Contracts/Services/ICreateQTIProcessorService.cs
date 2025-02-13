using DataTransferObjects.Creation;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories;

public interface ICreateQTIProcessorService
{
    public Task<QTITest> PreProcessAsync(QTITestCreationDTO test);
    public Task<QTITest> PostProcessAsync(QTITest test);
    public Task<String> ConvertQTIPackageAsync(string Base64Package);
}
