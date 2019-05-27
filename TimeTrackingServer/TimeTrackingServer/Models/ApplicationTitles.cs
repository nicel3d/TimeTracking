using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Models
{
    public partial class ApplicationTitles : IIdentifier
    {
        public ApplicationTitles()
        {
            ApplicationTitleToGroup = new HashSet<ApplicationTitleToGroup>();
        }

        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ApplicationId { get; set; }
        public StateEnum State { get; set; }
        public ModeEnum Mode { get; set; }

        public virtual Applications Application { get; set; }
        public virtual ICollection<ApplicationTitleToGroup> ApplicationTitleToGroup { get; set; }
    }
}
