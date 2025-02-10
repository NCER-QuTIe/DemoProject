using Entities.Enums;
using Entities.Models;

namespace Entities.Models.Configurations;

public static class ExternalTestConfiguration
{
    public static List<ExternalTest> InitialData()
    {
        return new List<ExternalTest>
        {
            new ExternalTest
            {
                Name = "Test 1",
                Description = "Description for Test 1",
                Url = "https://codeforces.com/",
                Tags = new[] { "tag1", "tag2" },
                Status = TestStatusEnum.Active,
                Uploaded = DateTime.Now
            },
            new ExternalTest
            {
                Name = "Test 2",
                Description = "Description for Test 2",
                Url = "https://leetcode.com/",
                Tags = new[] { "tag3", "tag4" },
                Status = TestStatusEnum.InActive,
                Uploaded = DateTime.Now.AddDays(-1)
            }
        };
    }

}
