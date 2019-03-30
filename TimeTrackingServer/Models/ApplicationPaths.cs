using System;
using System.Collections.Generic;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationPaths
    {
        public int ApplicationId { get; set; }
        public int StaffId { get; set; }
        public string Path { get; set; }
        public int Id { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
