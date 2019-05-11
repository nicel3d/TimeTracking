using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Models
{
    public partial class Groups : IIdentifier
    {
        public Groups()
        {
            ApplicationTitleToGroup = new HashSet<ApplicationTitleToGroup>();
            ApplicationToGroup = new HashSet<ApplicationToGroup>();
            StaffToGroup = new HashSet<StaffToGroup>();
        }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Status { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ApplicationTitleToGroup> ApplicationTitleToGroup { get; set; }
        public virtual ICollection<ApplicationToGroup> ApplicationToGroup { get; set; }
        public virtual ICollection<StaffToGroup> StaffToGroup { get; set; }
    }
}
