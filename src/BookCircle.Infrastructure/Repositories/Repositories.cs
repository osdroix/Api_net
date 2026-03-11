using BookCircle.Domain.Entities;
using BookCircle.Domain.Interfaces;
using BookCircle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookCircle.Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username) =>
        db.Users.FirstOrDefaultAsync(u => u.Username == username);

    // Busca un usuario por su ID
    public Task<User?> GetByIdAsync(int id) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByEmailAsync(string email) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }
}

public class ClubRepository(AppDbContext db) : IClubRepository
{
    public async Task<IEnumerable<Club>> GetAllAsync(int skip, int limit) =>
        await db.Clubs.Skip(skip).Take(limit).ToListAsync();

    public Task<Club?> GetByIdAsync(int id) =>
        db.Clubs.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Club> CreateAsync(Club club)
    {
        db.Clubs.Add(club);
        await db.SaveChangesAsync();
        return club;
    }

    public async Task<Club> UpdateAsync(Club club)
    {
        db.Clubs.Update(club);
        await db.SaveChangesAsync();
        return club;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var club = await db.Clubs.FindAsync(id);
        if (club is null) return false;
        db.Clubs.Remove(club);
        await db.SaveChangesAsync();
        return true;
    }

    // Agrega un usuario como miembro de un club
    public async Task AddMemberAsync(ClubMember member)
    {
        db.ClubMembers.Add(member);
        await db.SaveChangesAsync();
    }

    // Elimina a un usuario de un club
    public async Task RemoveMemberAsync(int clubId, int userId)
    {
        var member = await db.ClubMembers.FindAsync(clubId, userId);
        if (member != null)
        {
            db.ClubMembers.Remove(member);
            await db.SaveChangesAsync();
        }
    }

    // Obtiene la lista de usuarios que son miembros de un club
    public async Task<IEnumerable<User>> GetMembersAsync(int clubId) =>
        await db.ClubMembers
            .Where(cm => cm.ClubId == clubId)
            .Include(cm => cm.User) // Incluimos la entidad User para retornar sus datos
            .Select(cm => cm.User)
            .ToListAsync();

    // Verifica si un usuario ya es miembro de un club
    public async Task<bool> IsMemberAsync(int clubId, int userId) =>
        await db.ClubMembers.AnyAsync(cm => cm.ClubId == clubId && cm.UserId == userId);
}

public class BookRepository(AppDbContext db) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetByClubIdAsync(int clubId, int skip, int limit) =>
        await db.Books.Where(b => b.ClubId == clubId).Skip(skip).Take(limit).ToListAsync();

    public Task<Book?> GetByIdAsync(int clubId, int bookId) =>
        db.Books.FirstOrDefaultAsync(b => b.ClubId == clubId && b.Id == bookId);

    public async Task<Book> CreateAsync(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        db.Books.Update(book);
        await db.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(int bookId)
    {
        var book = await db.Books.FindAsync(bookId);
        if (book is null) return false;
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return true;
    }
}

public class ReviewRepository(AppDbContext db) : IReviewRepository
{
    public async Task<IEnumerable<Review>> GetByBookIdAsync(int clubId, int bookId) =>
        await db.Reviews.Where(r => r.ClubId == clubId && r.BookId == bookId).ToListAsync();

    public Task<Review?> GetByIdAsync(int id) =>
        db.Reviews.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Review> CreateAsync(Review review)
    {
        db.Reviews.Add(review);
        await db.SaveChangesAsync();
        return review;
    }

    public async Task<Review> UpdateAsync(Review review)
    {
        db.Reviews.Update(review);
        await db.SaveChangesAsync();
        return review;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await db.Reviews.FindAsync(id);
        if (review is null) return false;
        db.Reviews.Remove(review);
        await db.SaveChangesAsync();
        return true;
    }
}

public class MeetingRepository(AppDbContext db) : IMeetingRepository
{
    public async Task<IEnumerable<Meeting>> GetByClubIdAsync(int clubId) =>
        await db.Meetings.Where(m => m.ClubId == clubId).ToListAsync();

    public Task<Meeting?> GetByIdAsync(int clubId, int meetingId) =>
        db.Meetings.FirstOrDefaultAsync(m => m.ClubId == clubId && m.Id == meetingId);

    public async Task<Meeting> CreateAsync(Meeting meeting)
    {
        db.Meetings.Add(meeting);
        await db.SaveChangesAsync();
        return meeting;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var meeting = await db.Meetings.FindAsync(id);
        if (meeting is null) return false;
        db.Meetings.Remove(meeting);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<MeetingAttendance> AddAttendanceAsync(MeetingAttendance attendance)
    {
        db.MeetingAttendances.Add(attendance);
        await db.SaveChangesAsync();
        return attendance;
    }
}
