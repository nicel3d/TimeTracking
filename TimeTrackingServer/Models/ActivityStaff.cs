using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ActivityStaff
    {
        public int Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ApplicationTitle { get; set; }
        public int? StaffId { get; set; }
        public string ImageUrlBig { get; set; }
        public string ImageUrlSmall { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
