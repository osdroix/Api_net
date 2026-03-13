namespace BookCircle.Application.DTOs;

// ── Auth ──────────────────────────────────────────────────────────────────────

public record LoginRequest(string Username, string Password);

public record TokenResponse(string AccessToken, string TokenType = "bearer");

public record UserCreateDto(string Email, string Username, string Password, string FullName);

public record UserOutDto(int Id, string Email, string Username, string? FullName, DateTime CreatedAt);

// ── Club ──────────────────────────────────────────────────────────────────────

public record ClubCreateDto(string Name, string Description, string? FavoriteGenre = null, int Members = 0);

public record ClubOutDto(int Id, string Name, string Description, int MemberCount);

// ── Book ──────────────────────────────────────────────────────────────────────

public record BookCreateDto(int ClubId, string Title, string Author, int Votes = 0, int Progress = 0);

public record BookOutDto(int Id, int ClubId, string Title, string Author, int Votes, int Progress);

// ── Review ────────────────────────────────────────────────────────────────────

public record ReviewCreateDto(int ClubId, int BookId, int UserId, int Rating, string Comment);

public record ReviewOutDto(int Id, int ClubId, int BookId, int UserId, int Rating, string Comment);

public record ReviewUpdateDto(int Id, int ClubId, int BookId, int Rating, string Comment);

// ── Meeting ───────────────────────────────────────────────────────────────────

public record MeetingCreateDto(
    int BookId,
    int ClubId,
    string BookTitle,
    string? ScheduledAt,
    int? Duration,
    string Location,
    string LocationUrl,
    string Description,
    string? CreatedBy,
    int? AttendeeCount,
    string? Status,
    bool? IsVirtual,
    string VirtualMeetingUrl);

public record MeetingOutDto(
    int Id,
    int ClubId,
    int BookId,
    string BookTitle,
    string? ScheduledAt,
    int? Duration,
    string Location,
    string LocationUrl,
    string Description,
    string? CreatedBy,
    int? AttendeeCount,
    string? Status,
    bool? IsVirtual,
    string VirtualMeetingUrl);

// ── Attendance ────────────────────────────────────────────────────────────────

public record MeetingAttendanceCreateDto(int UserId, string Status);
