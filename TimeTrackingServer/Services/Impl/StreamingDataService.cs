using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            Applications application = _dbContext.Applications
                .Where(x => x.Alias == applicationAlias)
                .FirstOrDefault();

            if (application == null)
            {
                application = new Applications()
                {
                    Alias = applicationAlias,
                    Name = applicationAlias != null ? Regex.Replace(applicationAlias, @"\..*", "") : "",
                    State = StateEnum.Neutral,
                };

                _dbContext.Applications.Add(application);
                await _dbContext.SaveChangesAsync();
            }

            return application;
        }

        public async Task<Staff> AddAndGetStaff(string staffAlias)
        {
            Staff staff = _dbContext.Staff
                .Where(x => x.Alias == staffAlias)
                .FirstOrDefault();

            if (staff == null)
            {
                staff = new Staff()
                {
                    Alias = staffAlias,
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

            ActivityStaff activityStaff = new ActivityStaff()
            {
                ApplicationTitle = streamingDataRequest.ApplicationTitle,
                ImageUrlBig = streamingDataRequest.ApplicationImage,
                ImageUrlSmall = ImageHelper.GetSmallImageFrombase64String(streamingDataRequest.ApplicationImage),
                StaffId = staff.Id,
                UpdatedAt = DateTimeHelper.UnixTimeStampToDateTime(streamingDataRequest.ActivityTime)
            };

            _dbContext.ActivityStaff.Add(activityStaff);
            await _dbContext.SaveChangesAsync();
        }
    }
}
