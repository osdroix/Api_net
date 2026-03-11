using AutoMapper;
using BookCircle.Application.DTOs;
using BookCircle.Application.Services.Interfaces;
using BookCircle.Domain.Entities;
using BookCircle.Domain.Interfaces;

namespace BookCircle.Application.Services.Implementations;

public class ClubService(IClubRepository repo, IUserRepository userRepo, IMapper mapper) : IClubService
{
    public async Task<IEnumerable<ClubOutDto>> GetAllAsync(int skip, int limit)
    {
        var clubs = await repo.GetAllAsync(skip, limit);
        return mapper.Map<IEnumerable<ClubOutDto>>(clubs);
    }

    public async Task<ClubOutDto> GetByIdAsync(int id)
    {
        var club = await repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Club {id} not found.");
        return mapper.Map<ClubOutDto>(club);
    }

    public async Task<ClubOutDto> CreateAsync(ClubCreateDto dto)
    {
        var club = mapper.Map<Club>(dto);
        var created = await repo.CreateAsync(club);
        return mapper.Map<ClubOutDto>(created);
    }

    public async Task<ClubOutDto> UpdateAsync(int id, ClubCreateDto dto)
    {
        var club = await repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Club {id} not found.");
        
        club.Name = dto.Name;
        club.Description = dto.Description;
        club.FavoriteGenre = dto.FavoriteGenre;
        // club.Members se actualiza a través de Add/RemoveMemberAsync, por lo general no lo actualizamos manualmente vía UpdateDto a menos que sea una corrección administrativa.
        // Asumimos que dto.Members se ignora o se usa como anulación. Lo mantenemos por ahora, pero ten en cuenta que AddMember lo incrementa.
        
        var updated = await repo.UpdateAsync(club);
        return mapper.Map<ClubOutDto>(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        if (!deleted) throw new KeyNotFoundException($"Club {id} not found.");
    }

    // Permite a un usuario unirse a un club
    public async Task AddMemberAsync(int clubId, int userId)
    {
        // Verificamos que el club y el usuario existan
        var club = await repo.GetByIdAsync(clubId) ?? throw new KeyNotFoundException($"Club {clubId} not found.");
        var user = await userRepo.GetByIdAsync(userId) ?? throw new KeyNotFoundException($"User {userId} not found.");

        // Verificamos si ya es miembro
        if (await repo.IsMemberAsync(clubId, userId))
            throw new InvalidOperationException($"User {userId} is already a member of club {clubId}.");

        // Creamos la relación
        await repo.AddMemberAsync(new ClubMember { ClubId = clubId, UserId = userId });

        // Incrementamos el contador de miembros del club
        club.Members++;
        await repo.UpdateAsync(club);
    }

    // Permite a un usuario salir de un club
    public async Task RemoveMemberAsync(int clubId, int userId)
    {
        var club = await repo.GetByIdAsync(clubId) ?? throw new KeyNotFoundException($"Club {clubId} not found.");

        // Verificamos que sea miembro antes de eliminar
        if (!await repo.IsMemberAsync(clubId, userId))
            throw new KeyNotFoundException($"User {userId} is not a member of club {clubId}.");

        await repo.RemoveMemberAsync(clubId, userId);

        // Decrementamos el contador de miembros, asegurando que no sea negativo
        club.Members--;
        if (club.Members < 0) club.Members = 0;
        await repo.UpdateAsync(club);
    }

    // Obtiene los miembros de un club y los mapea a DTOs
    public async Task<IEnumerable<UserOutDto>> GetMembersAsync(int clubId)
    {
        var club = await repo.GetByIdAsync(clubId) ?? throw new KeyNotFoundException($"Club {clubId} not found.");
        var members = await repo.GetMembersAsync(clubId);
        return mapper.Map<IEnumerable<UserOutDto>>(members);
    }
}
