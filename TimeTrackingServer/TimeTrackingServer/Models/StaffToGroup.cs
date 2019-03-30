using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackingServer.Models
{
    public partial class StaffToGroup
    {
        [Key, Column(Order = 0)]
        public int StaffId { get; set; }
        [Key, Column(Order = 1)]
        public int GroupId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
