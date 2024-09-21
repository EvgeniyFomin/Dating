using AutoMapper;
using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.MappingProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, MemberDto>()
                .ForMember(x => x.MainPhotoUrl, opt => opt.MapFrom(dest => dest.Photos.SingleOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
