using System;
using System.Collections.Generic;
using TimeTrackingServer.Constants;

namespace TimeTrackingServer.Models
{
    public partial class Applications
    {
        public Applications()
        {
            ActivityStaff = new HashSet<ActivityStaff>();
            ApplicationTitles = new HashSet<ApplicationTitles>();
            ApplicationToGroup = new HashSet<ApplicationToGroup>();
        }

        public int Id { get; set; }
        public string Caption { get; set; }
        public DateTime UpdatedAt { get; set; }
        public StateEnum State { get; set; }

        public virtual ICollection<ActivityStaff> ActivityStaff { get; set; }
        public virtual ICollection<ApplicationTitles> ApplicationTitles { get; set; }
        public virtual ICollection<ApplicationToGroup> ApplicationToGroup { get; set; }
    }
}
