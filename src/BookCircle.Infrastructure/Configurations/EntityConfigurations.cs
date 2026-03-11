using BookCircle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCircle.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FullName).HasMaxLength(120);
    }
}

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(500);
        builder.Property(c => c.FavoriteGenre).HasMaxLength(100);
        builder.HasMany(c => c.Books).WithOne(b => b.Club).HasForeignKey(b => b.ClubId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Meetings).WithOne(m => m.Club).HasForeignKey(m => m.ClubId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClubMemberConfiguration : IEntityTypeConfiguration<ClubMember>
{
    public void Configure(EntityTypeBuilder<ClubMember> builder)
    {
        // Clave compuesta: un usuario solo puede estar una vez en un club específico
        builder.HasKey(cm => new { cm.ClubId, cm.UserId });
        
        // Relación con Club
        builder.HasOne(cm => cm.Club)
            .WithMany(c => c.ClubMembers)
            .HasForeignKey(cm => cm.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Relación con User
        builder.HasOne(cm => cm.User)
            .WithMany(u => u.ClubMembers)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Author).IsRequired().HasMaxLength(100);
        builder.HasMany(b => b.Reviews).WithOne(r => r.Book).HasForeignKey(r => r.BookId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Comment).IsRequired().HasMaxLength(1000);
    }
}

public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.BookTitle).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Location).HasMaxLength(300);
        builder.Property(m => m.LocationUrl).HasMaxLength(500);
        builder.Property(m => m.Description).HasMaxLength(1000);
        builder.Property(m => m.VirtualMeetingUrl).HasMaxLength(500);
        builder.HasMany(m => m.Attendances).WithOne(a => a.Meeting).HasForeignKey(a => a.MeetingId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class MeetingAttendanceConfiguration : IEntityTypeConfiguration<MeetingAttendance>
{
    public void Configure(EntityTypeBuilder<MeetingAttendance> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Status).HasConversion<string>().IsRequired();
    }
}
