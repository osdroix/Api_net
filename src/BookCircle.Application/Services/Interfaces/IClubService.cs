using BookCircle.Application.DTOs;

namespace BookCircle.Application.Services.Interfaces;

public interface IClubService
{
    Task<IEnumerable<ClubOutDto>> GetAllAsync(int skip, int limit);
    Task<ClubOutDto> GetByIdAsync(int id);
    Task<ClubOutDto> CreateAsync(ClubCreateDto dto);
    Task<ClubOutDto> UpdateAsync(int id, ClubCreateDto dto);
    Task DeleteAsync(int id);
    // Gestión de miembros
    Task AddMemberAsync(int clubId, int userId);
    Task RemoveMemberAsync(int clubId, int userId);
    Task<IEnumerable<UserOutDto>> GetMembersAsync(int clubId);
}
