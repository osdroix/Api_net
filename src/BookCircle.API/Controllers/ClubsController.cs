using BookCircle.Application.DTOs;
using BookCircle.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCircle.API.Controllers;

[ApiController]
[Route("clubs")]
[Authorize]
public class ClubsController(IClubService clubService) : ControllerBase
{
    /// <summary>Listar todos los clubes</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClubOutDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int limit = 100)
        => Ok(await clubService.GetAllAsync(skip, limit));

    /// <summary>Crear un club</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClubOutDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] ClubCreateDto dto)
    {
        var club = await clubService.CreateAsync(dto);
        return StatusCode(StatusCodes.Status201Created, club);
    }

    /// <summary>Obtener un club por ID</summary>
    [HttpGet("{clubId:int}")]
    [ProducesResponseType(typeof(ClubOutDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int clubId)
        => Ok(await clubService.GetByIdAsync(clubId));

    /// <summary>Actualizar un club</summary>
    [HttpPut("{clubId:int}")]
    [ProducesResponseType(typeof(ClubOutDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int clubId, [FromBody] ClubCreateDto dto)
        => Ok(await clubService.UpdateAsync(clubId, dto));

    /// <summary>Eliminar un club</summary>
    [HttpDelete("{clubId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int clubId)
    {
        await clubService.DeleteAsync(clubId);
        return NoContent();
    }

    /// <summary>Unirse a un club</summary>
    /// <remarks>Permite al usuario autenticado unirse al club especificado.</remarks>
    [HttpPost("{clubId:int}/members")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinClub(int clubId)
    {
        // Obtenemos el ID del usuario desde el token JWT
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
        await clubService.AddMemberAsync(clubId, userId);
        return NoContent();
    }

    /// <summary>Listar miembros del club</summary>
    /// <remarks>Obtiene la lista de usuarios que pertenecen al club.</remarks>
    [HttpGet("{clubId:int}/members")]
    [ProducesResponseType(typeof(IEnumerable<UserOutDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMembers(int clubId)
        => Ok(await clubService.GetMembersAsync(clubId));

    /// <summary>Eliminar un miembro del club</summary>
    /// <remarks>Elimina al usuario especificado del club. (Sin restricciones de permisos por ahora).</remarks>
    [HttpDelete("{clubId:int}/members/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LeaveClub(int clubId, int userId)
    {
        // Validamos que el usuario que intenta salir sea el mismo que está autenticado
        // (Comentado para permitir eliminar a cualquier miembro sin restricción por ahora)
        /*
        var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
        if (currentUserId != userId)
        {
            return Forbid();
        }
        */

        await clubService.RemoveMemberAsync(clubId, userId);
        return NoContent();
    }
}
