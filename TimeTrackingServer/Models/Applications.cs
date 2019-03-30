using System;
using System.Collections.Generic;
using TimeTrackingServer.Constants;

namespace TimeTrackingServer.Models
{
    public partial class Applications
    {
        public Applications()
        {
            ApplicationPaths = new HashSet<ApplicationPaths>();
            ApplicationTitles = new HashSet<ApplicationTitles>();
            ApplicationToGroup = new HashSet<ApplicationToGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StateEnum State { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<ApplicationPaths> ApplicationPaths { get; set; }
        public virtual ICollection<ApplicationTitles> ApplicationTitles { get; set; }
        public virtual ICollection<ApplicationToGroup> ApplicationToGroup { get; set; }
    }
}
