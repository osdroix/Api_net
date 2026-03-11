using BookCircle.Domain.Entities;
using BookCircle.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BookCircle.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Club> Clubs => Set<Club>();
    
    // Tabla intermedia para la relación muchos-a-muchos entre Clubes y Usuarios
    public DbSet<ClubMember> ClubMembers => Set<ClubMember>();
    
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingAttendance> MeetingAttendances => Set<MeetingAttendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ClubConfiguration());
        // Configuración de la tabla intermedia de miembros
        modelBuilder.ApplyConfiguration(new ClubMemberConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new MeetingConfiguration());
        modelBuilder.ApplyConfiguration(new MeetingAttendanceConfiguration());
    }
}
