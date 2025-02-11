using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmailSettings
{
    required public string SenderEmail { get; set; }
    required public string ReplyToEmail { get; set; }
    required public string ConfigurationSetName { get; set; }
}