using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TimeTrackingServer.Models
{
    public partial class DistributedSystemDevContext : DbContext
    {
        public DistributedSystemDevContext()
        {
        }

        public DistributedSystemDevContext(DbContextOptions<DistributedSystemDevContext> options)
            : base(options)
        {
        }

        protected DistributedSystemDevContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityStaff> ActivityStaff { get; set; }
        public virtual DbSet<ApplicationTitleToGroup> ApplicationTitleToGroup { get; set; }
        public virtual DbSet<ApplicationTitles> ApplicationTitles { get; set; }
        public virtual DbSet<ApplicationToGroup> ApplicationToGroup { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffToGroup> StaffToGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack")
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<ActivityStaff>(entity =>
            {
                entity.ToTable("activity_staff");

                entity.HasIndex(e => new { e.ApplicationId, e.StaffId, e.UpdatedAt })
                    .HasName("activity_staff_application_id_staff_id_updated_at_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('activity_id_seq'::regclass)");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.ApplicationTitle).HasColumnName("application_title");

                entity.Property(e => e.ImageOrigin).HasColumnName("image_origin");

                entity.Property(e => e.ImageThumb).HasColumnName("image_thumb");

                entity.Property(e => e.StaffId).HasColumnName("staff_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ActivityStaff)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("activity_staff_applications_id_fk");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ActivityStaff)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("activity_staff_id_fk");
            });

            modelBuilder.Entity<ApplicationTitleToGroup>(entity =>
            {
                entity.ToTable("application_title_to_group");

                entity.HasIndex(e => e.Id)
                    .HasName("application_title_to_group_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => new { e.ApplicationId, e.GroupId })
                    .HasName("application_title_to_group_title_id_group_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.Mode).HasColumnName("mode");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationTitleToGroup)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("application_title_to_group_applications_id_fk");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationTitleToGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("application_title_to_group_groups_id_fk");
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

                entity.Property(e => e.Mode).HasColumnName("mode");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Title).HasColumnName("title");

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

                entity.Property(e => e.State).HasColumnName("state");

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

                entity.HasIndex(e => e.Caption)
                    .HasName("applications_caption_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasColumnName("caption")
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    //.HasConversion( // custom converter
                    //    v => v.ToString(),
                    //    v => (StateEnum)Enum.Parse(typeof(StateEnum), v))
                    .HasColumnName("state");

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

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.ToTable("settings");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status).HasColumnName("status");

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

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("staff");

                entity.HasIndex(e => e.Caption)
                    .HasName("users_alias_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('users_id_seq'::regclass)");

                entity.Property(e => e.ActivityFirst).HasColumnName("activity_first");

                entity.Property(e => e.ActivityLast).HasColumnName("activity_last");

                entity.Property(e => e.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(100);

                entity.Property(e => e.RangeLastActivityTime)
                    .HasColumnName("range_last_activity_time")
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
