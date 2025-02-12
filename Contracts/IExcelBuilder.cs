using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.TestResults;

namespace Contracts;

public interface IExcelBuilder
{
    public Task GenerateExcelAsync(string outputPath, TestResponseBundleDTO testResponseBundle);
}
