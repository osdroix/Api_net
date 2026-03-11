using BookCircle.Domain.Entities;

namespace BookCircle.Domain.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAllAsync(int skip, int limit);
    Task<Club?> GetByIdAsync(int id);
    Task<Club> CreateAsync(Club club);
    Task<Club> UpdateAsync(Club club);
    Task<bool> DeleteAsync(int id);
    // Gestión de miembros
    Task AddMemberAsync(ClubMember member);
    Task RemoveMemberAsync(int clubId, int userId);
    Task<IEnumerable<User>> GetMembersAsync(int clubId);
    Task<bool> IsMemberAsync(int clubId, int userId);
}
