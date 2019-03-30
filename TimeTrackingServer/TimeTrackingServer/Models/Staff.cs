using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class Staff
    {
        public Staff()
        {
            ActivityStaff = new HashSet<ActivityStaff>();
            ApplicationPaths = new HashSet<ApplicationPaths>();
            StaffToGroup = new HashSet<StaffToGroup>();
        }

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Status { get; set; }
        public string Phone { get; set; }
        public DateTime? ActivityFirst { get; set; }
        public DateTime? ActivityLast { get; set; }
        public string AvatarUrl { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<ActivityStaff> ActivityStaff { get; set; }
        public virtual ICollection<ApplicationPaths> ApplicationPaths { get; set; }
        public virtual ICollection<StaffToGroup> StaffToGroup { get; set; }
    }
}
