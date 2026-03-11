namespace BookCircle.Domain.Entities;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? FavoriteGenre { get; set; }
    // Número de miembros actuales (se actualiza cuando alguien se une o sale)
    public int Members { get; set; } = 0;

    // Relación con los miembros del club
    public ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
    
    // Relación con los libros del club
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
}
