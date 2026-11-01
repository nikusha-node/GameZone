using AutoMapper;
using GameZone.DTOs.Auth;
using GameZone.Models;

namespace GameZone.Mappings
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}