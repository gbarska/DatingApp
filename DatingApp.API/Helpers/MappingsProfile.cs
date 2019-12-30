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
                        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));       
            CreateMap<Photo, PhotosForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<Photo, PhotosForReturnDTO>();
            CreateMap<User,UserWhithPhotoForApprovalDTO>();
                // .ForMember(dest => dest.KnownAs, opt => opt.MapFrom(src => src.User.KnownAs));
            CreateMap<PhotosForCreationDTO, Photo>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<Message, MessageToReturnDTO>()
             .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
             .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();
                   
            
        }
    }
}