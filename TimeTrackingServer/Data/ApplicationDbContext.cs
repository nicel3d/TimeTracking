using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserModel> User { get; set; }

        public virtual DbSet<ActivityStaff> ActivityStaff { get; set; }
        public virtual DbSet<ApplicationPaths> ApplicationPaths { get; set; }
        public virtual DbSet<ApplicationTitleToGroup> ApplicationTitleToGroup { get; set; }
        public virtual DbSet<ApplicationTitles> ApplicationTitles { get; set; }
        public virtual DbSet<ApplicationToGroup> ApplicationToGroup { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<ConfigToDayOfWeek> ConfigToDayOfWeek { get; set; }
        public virtual DbSet<Configs> Configs { get; set; }
        public virtual DbSet<ConfigDayOfWeek> ConfigDayOfWeek { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffToGroup> StaffToGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack")
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<ActivityStaff>(entity =>
            {
                entity.ToTable("activity_staff");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('activity_id_seq'::regclass)");

                entity.Property(e => e.ApplicationTitle).HasColumnName("application_title");

                entity.Property(e => e.ImageUrlBig).HasColumnName("image_url_big");

                entity.Property(e => e.ImageUrlSmall).HasColumnName("image_url_small");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ActivityStaff)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("activity_staff_id_fk");
            });

            modelBuilder.Entity<ApplicationPaths>(entity =>
            {
                entity.ToTable("application_paths");

                entity.HasIndex(e => e.Id)
                    .HasName("application_paths_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => new { e.ApplicationId, e.StaffId, e.Path })
                    .HasName("application_paths_application_id_user_id_path_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationPaths)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("application_paths_applications_id_fk");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ApplicationPaths)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("application_paths_staff_id_fk");
            });

            modelBuilder.Entity<ApplicationTitleToGroup>(entity =>
            {
                entity.ToTable("application_title_to_group");

                entity.HasIndex(e => e.Id)
                    .HasName("application_title_to_group_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(100);

                entity.Property(e => e.TitleId).HasColumnName("title_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationTitleToGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("application_title_to_group_groups_id_fk");

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.ApplicationTitleToGroup)
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("application_title_to_group_application_titles_id_fk");
            });

            modelBuilder.Entity<ApplicationTitles>(entity =>
            {
                entity.ToTable("application_titles");

                entity.HasIndex(e => e.Id)
                    .HasName("applications_title_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => new { e.Title, e.ApplicationId })
                    .HasName("applications_title_title_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('applications_title_id_seq'::regclass)");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationTitles)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("applications_title_applications_id_fk");
            });

            modelBuilder.Entity<ApplicationToGroup>(entity =>
            {
                entity.ToTable("application_to_group");

                entity.HasIndex(e => new { e.ApplicationId, e.GroupId })
                    .HasName("application_to_user_forbidden_application_id_user_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('application_to_user_forbidden_id_seq'::regclass)");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationToGroup)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("application_to_user_forbidden_applications_id_fk");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationToGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("application_to_user_forbidden_users_id_fk");
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.ToTable("applications");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<ConfigDayOfWeek>(entity =>
            {
                entity.ToTable("config_day_of_week");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('day_of_week_id_seq'::regclass)");

                entity.Property(e => e.DayNumber).HasColumnName("day_number");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<ConfigToDayOfWeek>(entity =>
            {
                entity.HasKey(e => new { e.ConfigId, e.DayOfWeekId })
                    .HasName("config_to_day_of_week_config_id_day_of_week_id_pk");

                entity.ToTable("config_to_day_of_week");

                entity.Property(e => e.ConfigId).HasColumnName("config_id");

                entity.Property(e => e.DayOfWeekId).HasColumnName("day_of_week_id");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.ConfigToDayOfWeek)
                    .HasForeignKey(d => d.ConfigId)
                    .HasConstraintName("config_to_day_of_week_configs_id_fk");

                entity.HasOne(d => d.DayOfWeek)
                    .WithMany(p => p.ConfigToDayOfWeek)
                    .HasForeignKey(d => d.DayOfWeekId)
                    .HasConstraintName("config_to_day_of_week_day_of_week_id_fk");
            });

            modelBuilder.Entity<Configs>(entity =>
            {
                entity.ToTable("configs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.TimeBreakFrom)
                    .HasColumnName("time_break_from")
                    .HasMaxLength(100);

                entity.Property(e => e.TimeBreakTo)
                    .HasColumnName("time_break_to")
                    .HasMaxLength(100);

                entity.Property(e => e.TimeTheadMiliseconds).HasColumnName("time_thead_miliseconds");

                entity.Property(e => e.TimeWorkingFrom)
                    .HasColumnName("time_working_from")
                    .HasMaxLength(100);

                entity.Property(e => e.TimeWorkingTo)
                    .HasColumnName("time_working_to")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.ToTable("groups");

                entity.HasIndex(e => e.Id)
                    .HasName("groups_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("groups_name_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("staff");

                entity.HasIndex(e => e.Alias)
                    .HasName("users_alias_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('users_id_seq'::regclass)");

                entity.Property(e => e.ActivityFirst).HasColumnName("activity_first");

                entity.Property(e => e.ActivityLast).HasColumnName("activity_last");

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasMaxLength(100);

                entity.Property(e => e.AvatarUrl)
                    .HasColumnName("avatar_url")
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<StaffToGroup>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.GroupId })
                    .HasName("user_to_group_user_id_group_id_pk");

                entity.ToTable("staff_to_group");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.StaffToGroup)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("staff_to_group_groups_id_fk");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffToGroup)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("staff_to_group_staff_id_fk");
            });
        }
    }
}
