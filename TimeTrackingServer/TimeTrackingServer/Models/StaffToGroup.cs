using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class StaffToGroup
    {
        public int StaffId { get; set; }
        public int GroupId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
