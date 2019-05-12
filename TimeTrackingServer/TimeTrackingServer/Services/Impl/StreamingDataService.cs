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

        public StreamingDataService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Applications> AddAndGetApplication(string applicationAlias)
        {
            Applications application = await _dbContext.Applications
                .Where(x => x.Caption == applicationAlias)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                application = new Applications()
                {
                    Caption = applicationAlias,
                    State = StateEnum.Neutral
                };

                _dbContext.Applications.Add(application);
                await _dbContext.SaveChangesAsync();
            }

            return application;
        }

        public async Task<Staff> AddAndGetStaff(string staffAlias)
        {
            Staff staff = await _dbContext.Staff
                .Where(x => x.Caption == staffAlias)
                .FirstOrDefaultAsync();

            if (staff == null)
            {
                staff = new Staff()
                {
                    Caption = staffAlias,
                    Status = true
                };
                _dbContext.Staff.Add(staff);
                await _dbContext.SaveChangesAsync();
            }

            return staff;
        }

        public async Task AddActivity(StreamingDataRequest streamingDataRequest)
        {
            var staff = await AddAndGetStaff(streamingDataRequest.StaffAlias);
            var application = await AddAndGetApplication(streamingDataRequest.ApplicationAlias);

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

                _dbContext.ActivityStaff.Add(activityStaff);
                await _dbContext.SaveChangesAsync();
            }
        }
        //    public async Task AddApplicationTitle(int applicationId, string applicationTitle)
        //    {
        //        ApplicationTitles applicationTitles = new ApplicationTitles()
        //        {
        //            ApplicationId = applicationId,
        //            Title = applicationTitle
        //        };
        //        _dbContext.ApplicationTitles.Add(applicationTitles);
        //        await _dbContext.SaveChangesAsync();
        //    }
    }
}
