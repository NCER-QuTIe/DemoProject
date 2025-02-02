using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Configurations;

public static class FeedbackConfiguration
{
    public static List<Feedback> InitialData()
    {
        return new List<Feedback>
        {
            new Feedback
            {
                Email = "test@gmail.com",
                Message = "This is a test feedback"
            },
            new Feedback
            {
                Email = "",
                Message = "This is a test feedback 2"
            }
        };
    }
}
