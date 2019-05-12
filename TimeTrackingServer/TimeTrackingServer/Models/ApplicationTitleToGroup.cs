using System;
using System.Collections.Generic;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationTitleToGroup : IIdentifier
    {
        public int? Id { get; set; }
        public int? ApplicationTitleId { get; set; }
        public int? GroupId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StateEnum State { get; set; }

        public virtual ApplicationTitles ApplicationTitle { get; set; }
        public virtual Groups Group { get; set; }
    }
}
