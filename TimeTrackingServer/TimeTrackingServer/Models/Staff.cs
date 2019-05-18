using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class Staff
    {
        public Staff()
        {
            ActivityStaff = new HashSet<ActivityStaff>();
            StaffToGroup = new HashSet<StaffToGroup>();
        }

        public int Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Status { get; set; }
        public DateTime? ActivityFirst { get; set; }
        public string RangeLastActivityTime { get; set; }
        public DateTime? ActivityLast { get; set; }
        public string Caption { get; set; }

        public virtual ICollection<ActivityStaff> ActivityStaff { get; set; }
        public virtual ICollection<StaffToGroup> StaffToGroup { get; set; }
    }
}
