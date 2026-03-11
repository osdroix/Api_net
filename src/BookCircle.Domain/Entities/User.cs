namespace BookCircle.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? FullName { get; set; }
    // Fecha de registro
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relación con los clubes a los que pertenece el usuario
    public ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}
