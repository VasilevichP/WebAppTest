using Microsoft.EntityFrameworkCore;
using WebAppTest.Entities;

namespace WebAppTest.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<QuestPhoto> QuestPhotos { get; set; }
    
    [Obsolete("Obsolete")]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasDefaultValue("User");
        
        modelBuilder.Entity<User>()
            .HasCheckConstraint("CK_User_Email_Format", "Email LIKE '%_@__%.__%'");
        
        modelBuilder.Entity<User>()
            .HasCheckConstraint("CK_User_Phone_Format", "Phone LIKE '+%' AND LENGTH(Phone) >= 10");
        
        modelBuilder.Entity<Quest>()
            .HasCheckConstraint("CK_Quest_MinParticipants", "MaxParticipants >= 1");
        
        modelBuilder.Entity<Quest>()
            .HasCheckConstraint("CK_Quest_DurationNonNegative", "DurationMinutes >= 0");
        
        modelBuilder.Entity<Quest>()
            .HasCheckConstraint("CK_Quest_FearLevel", "FearLevel >= 0 AND FearLevel <= 5");
        
        modelBuilder.Entity<Quest>()
            .HasCheckConstraint("CK_Quest_DifficultyLevel", "DifficultyLevel >= 0 AND DifficultyLevel <= 5");
        
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);;
    
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Quest)
            .WithMany(q => q.Bookings)
            .HasForeignKey(b => b.QuestId)
            .OnDelete(DeleteBehavior.Cascade);;
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);;
    
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Quest)
            .WithMany(q => q.Reviews)
            .HasForeignKey(r => r.QuestId)
            .OnDelete(DeleteBehavior.Cascade);;
       
        modelBuilder.Entity<Review>()
            .HasCheckConstraint("CK_Review_Rating", "Rating >= 0 AND Rating <= 10");
        
        modelBuilder.Entity<QuestPhoto>()
            .HasOne(p => p.Quest)
            .WithMany(q => q.Photos)
            .HasForeignKey(p => p.QuestId)
            .OnDelete(DeleteBehavior.Cascade);;
    }
}