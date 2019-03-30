using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackingServer.Models
{
    public partial class ConfigToDayOfWeek
    {
        [Key, Column(Order = 0)]
        public int ConfigId { get; set; }
        [Key, Column(Order = 1)]
        public int DayOfWeekId { get; set; }

        public virtual Configs Config { get; set; }
        public virtual ConfigDayOfWeek DayOfWeek { get; set; }
    }
}
