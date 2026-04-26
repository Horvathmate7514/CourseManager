using CourseManager.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Context
{
    public class NeptunDbContext : DbContext
    {
        public NeptunDbContext(DbContextOptions<NeptunDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(u => u.RegisteredCourses)
                .UsingEntity(j => j.ToTable("CourseStudents"));

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Teachers)
                .WithMany(u => u.TaughtCourses)
                .UsingEntity(j => j.ToTable("CourseTeachers"));
        }

    }
}
