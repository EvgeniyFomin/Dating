using AutoMapper;
using Dating.Core.Dtos;
using Dating.Core.Extensions;
using Dating.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace Dating.API.MappingProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.GetAge()))

                .ForMember(dest => dest.MainPhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain)!.Url));

            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(dest => MapToDateOnlyFromString(dest.DateOfBirth)));

            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        }

        private static DateOnly MapToDateOnlyFromString(string? s)
        {
            var res = s?.Split("T")[0];

            _ = DateOnly.TryParse(res, out DateOnly date);

            return date;
        }
    }
}
