using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class Configs
    {
        public Configs()
        {
            ConfigToDayOfWeek = new HashSet<ConfigToDayOfWeek>();
        }

        public int Id { get; set; }
        public string TimeBreakFrom { get; set; }
        public string TimeBreakTo { get; set; }
        public int? TimeTheadMiliseconds { get; set; }
        public bool? Status { get; set; }
        public string TimeWorkingFrom { get; set; }
        public string TimeWorkingTo { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ConfigToDayOfWeek> ConfigToDayOfWeek { get; set; }
    }
}
