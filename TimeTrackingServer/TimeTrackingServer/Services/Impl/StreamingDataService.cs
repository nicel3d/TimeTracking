using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Data;
using TimeTrackingServer.Helpers;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services.Impl
{
    public class StreamingDataService : IStreamingDataService
    {
        ApplicationDbContext _dbContext;
        private IStaffService _staffService;
        private IApplicationsService _applicationsService;
        private IActivityStaffService _activityStaffService;

        public StreamingDataService(
            ApplicationDbContext dbContext,
            IStaffService staffService,
            IApplicationsService applicationsService,
            IActivityStaffService activityStaffService)
        {
            _dbContext = dbContext;
            _staffService = staffService;
            _applicationsService = applicationsService;
            _activityStaffService = activityStaffService;
        }

        public async Task AddActivity(StreamingDataRequest streamingDataRequest)
        {
            var staff = await _staffService.GetOrAddStaffByAlias(streamingDataRequest.StaffAlias);
            var application = await _applicationsService.GetOrAddApplicationByAlias(streamingDataRequest.ApplicationAlias);

            if (staff != null && application != null)
            {
                ActivityStaff activityStaff = new ActivityStaff()
                {
                    ApplicationTitle = streamingDataRequest.ApplicationTitle,
                    ImageOrigin = streamingDataRequest.ApplicationImage,
                    ImageThumb = ImageHelper.GetSmallImageFromBytes(streamingDataRequest.ApplicationImage),
                    StaffId = staff?.Id,
                    ApplicationId = application?.Id,
                    UpdatedAt = DateTimeHelper.UnixTimeStampToDateTime(streamingDataRequest.ActivityTime)
                };

                await _activityStaffService.Post(activityStaff);
            }
        }
    }
}
