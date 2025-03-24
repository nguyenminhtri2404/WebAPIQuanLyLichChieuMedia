using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API_TEST.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        public DbSet<Media> Media { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasMany(m => m.Schedules)
                      .WithOne(s => s.Media)
                      .HasForeignKey(s => s.MediaId);

                entity.HasMany(m => m.TimeSlots)
                      .WithOne(t => t.Media)
                      .HasForeignKey(t => t.MediaId);

            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(s => s.Frequency).HasConversion<string>();
                entity.HasOne(s => s.Media)
                      .WithMany(m => m.Schedules)
                      .HasForeignKey(s => s.MediaId);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {

                // Value converter for DaysOfWeek
                ValueConverter<List<DayOfWeek>, string> daysOfWeekConverter = new(
                    v => string.Join(',', v.Select(d => d.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => Enum.Parse<DayOfWeek>(d)).ToList()
                );

                entity.Property(t => t.DaysOfWeek)
                      .HasConversion(daysOfWeekConverter);
            });

        }

    }
}
