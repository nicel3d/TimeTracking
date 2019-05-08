using System;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Models
{
    public partial class ActivityStaff : ITWithUpdateAt
    {
        public int Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ApplicationTitle { get; set; }
        public int? StaffId { get; set; }
        public byte[] ImageOrigin { get; set; }
        public byte[] ImageThumb { get; set; }
        public int? ApplicationId { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
