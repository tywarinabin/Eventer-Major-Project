using AttendenceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.DatabaseContext
{
    public sealed class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
        }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=EventEPAM;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Keep cascade delete for Event

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany()  // ⛔ No navigation property needed to prevent cycle
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // ✅ Disable cascade delete for User
        }
    }
}
