using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationTitles
    {
        public ApplicationTitles()
        {
            ApplicationTitleToGroup = new HashSet<ApplicationTitleToGroup>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ApplicationId { get; set; }

        public virtual Applications Application { get; set; }
        public virtual ICollection<ApplicationTitleToGroup> ApplicationTitleToGroup { get; set; }
    }
}
