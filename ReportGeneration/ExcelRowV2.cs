using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration;

public struct ExcelRowV2
{
    public string TestName { get; set; }
    public int ItemNumber { get; set; }
    public int QuestionNumber { get; set; }
    public string  StudentAnswer { get; set; }
    public string CorrectAnswer { get; set; }
    public int PointsReceived { get; set; }
    public int PointsMaximal { get; set; }
    public TimeOnly TimeTaken { get; set; }
    public DateTime DateStarted { get; set; }
    public DateTime DateEnded { get; set; }
    public bool Open { get; set; }
    public bool Correct { get; set; }
    public string Link { get; set; }
}
