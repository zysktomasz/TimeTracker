using Microsoft.EntityFrameworkCore;
using TimeTracker.Domain.Entities;

namespace TimeTracker.Persistance
{
    public class TimeTrackerDbContext : DbContext
    {
        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options)
            : base(options)
        {

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
