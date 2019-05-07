using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Data
{
    public class ApplicationDbContext : DistributedSystemDevContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<UserModel> User { get; set; }
    }
}
