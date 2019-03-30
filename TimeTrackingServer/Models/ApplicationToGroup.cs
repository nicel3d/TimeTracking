using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationToGroup
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public int? GroupId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string State { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Groups Group { get; set; }
    }
}
