using lab3_11.api.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentRoom> StudentRooms { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentRoom>()
            .HasOne(sr => sr.Student)
            .WithMany(s => s.StudentRooms)
            .HasForeignKey(sr => sr.StudentId);

        modelBuilder.Entity<StudentRoom>()
            .HasOne(sr => sr.Room)
            .WithMany(r => r.StudentRooms)
            .HasForeignKey(sr => sr.RoomId);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Room)
            .WithMany(ro => ro.Reviews)  // Відгук належить до кімнати
            .HasForeignKey(r => r.RoomId);
    }
}