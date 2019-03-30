using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationTitleToGroup
    {
        public int Id { get; set; }
        public int? TitleId { get; set; }
        public int? GroupId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string State { get; set; }

        public virtual Groups Group { get; set; }
        public virtual ApplicationTitles Title { get; set; }
    }
}
