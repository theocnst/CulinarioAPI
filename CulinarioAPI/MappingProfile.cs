using AutoMapper;
using CulinarioAPI.Dtos;
using CulinarioAPI.Models;

namespace CulinarioAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, UserCredentials>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileUpdateDto, UserProfile>();
        }
    }
}
