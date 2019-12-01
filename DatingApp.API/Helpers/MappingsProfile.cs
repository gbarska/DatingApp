using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.Domain.Models;
using System.Linq;

namespace DatingApp.API.Helpers
{
    public class MappingsProfiles : Profile
    {
        public MappingsProfiles()
        {
            CreateMap<User, UserForListDTO>()
                                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));;

            CreateMap<User, UserForDetailedDTO>()
                        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));;        

            CreateMap<Photo, PhotosForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<Photo, PhotosForReturnDTO>();
            CreateMap<PhotosForCreationDTO, Photo>();
            CreateMap<UserForRegisterDTO, User>();
            
        }
    }
}