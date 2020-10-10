using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Dtos;
using Tinder.API.Helper;
using Tinder.API.Models;

namespace Tinder.API.Helper
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
           => new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<User, UserForListDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom((src) => src.Birthday.CalculateAge()));

               cfg.CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom((src) => src.Birthday.CalculateAge())); 
               cfg.CreateMap<Photo, PhotoForDetailedDto>();
               cfg.CreateMap<UserForUpdate, User>()
                    .ForMember(model => model.Photos, opt => opt.Ignore());
               cfg.CreateMap<Photo, PhotoForReturnDto>();
               cfg.CreateMap<Photo, PhotoForCreationDto>();
               cfg.CreateMap<PhotoForCreationDto, Photo>();
               cfg.CreateMap<UserForRegisterDto, User>();
               cfg.CreateMap<MessageForCreationDto, Message>().ReverseMap();
               cfg.CreateMap<Message, MessageToReturnDto>()
                        .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                        .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
           }).CreateMapper();
    }
}
