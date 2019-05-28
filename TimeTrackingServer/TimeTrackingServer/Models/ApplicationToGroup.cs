using System;
using System.Collections.Generic;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationToGroup : IIdentifier
    {
        public int? Id { get; set; }
        public int ApplicationId { get; set; }
        public int GroupId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StateEnum State { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Groups Group { get; set; }
    }
}
