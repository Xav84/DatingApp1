using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
       public AutoMapperProfiles()
       {
            CreateMap<AppUser, MemberDto>()
            // Aller chercher dans l'url d'une photo dans Photos et le retourner dans MemberDto sous la prop PhotoUrl
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                // permet de retourner le resultat de CalculateAge() dans la prop Age de MemberDto
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
       } 
    }
}