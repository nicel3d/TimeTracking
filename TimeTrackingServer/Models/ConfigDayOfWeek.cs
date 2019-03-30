using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ConfigDayOfWeek
    {
        public ConfigDayOfWeek()
        {
            ConfigToDayOfWeek = new HashSet<ConfigToDayOfWeek>();
        }

        public int Id { get; set; }
        public int? DayNumber { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<ConfigToDayOfWeek> ConfigToDayOfWeek { get; set; }
    }
}
