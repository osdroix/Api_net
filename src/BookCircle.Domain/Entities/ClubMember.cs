namespace BookCircle.Domain.Entities;

/// <summary>
/// Representa la relación muchos-a-muchos entre usuarios y clubes.
/// Almacena cuándo se unió un usuario a un club.
/// </summary>
public class ClubMember
{
    // Clave foránea del club
    public int ClubId { get; set; }
    
    // Clave foránea del usuario
    public int UserId { get; set; }
    
    // Fecha en que el usuario se unió al club
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    public Club Club { get; set; } = null!;
    public User User { get; set; } = null!;
}
