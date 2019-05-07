using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ConfigToDayOfWeek
    {
        public int ConfigId { get; set; }
        public int DayOfWeekId { get; set; }

        public virtual Configs Config { get; set; }
        public virtual ConfigDayOfWeek DayOfWeek { get; set; }
    }
}
