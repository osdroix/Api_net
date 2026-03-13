using AutoMapper;
using BookCircle.Application.DTOs;
using BookCircle.Domain.Entities;
using BookCircle.Domain.Enums;

namespace BookCircle.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<User, UserOutDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        // Club
        CreateMap<ClubCreateDto, Club>();
        CreateMap<Club, ClubOutDto>()
            .ForCtorParam("MemberCount", opt => opt.MapFrom(src => src.Members));

        // Book
        CreateMap<BookCreateDto, Book>();
        CreateMap<Book, BookOutDto>();

        // Review
        CreateMap<ReviewCreateDto, Review>();
        CreateMap<Review, ReviewOutDto>();
        CreateMap<ReviewUpdateDto, Review>();

        // Meeting
        CreateMap<MeetingCreateDto, Meeting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Meeting, MeetingOutDto>();

        // MeetingAttendance
        CreateMap<MeetingAttendanceCreateDto, MeetingAttendance>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                Enum.Parse<AttendanceValue>(src.Status, true)));
    }
}
