using AutoMapper;
using DatingApp.Domain.DTOs;
using DatingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            
        }
    }
}