using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users
{
    internal class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            
            CreateMap<CreateUserRequest, User>();

            
            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore both null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));
        }
    }
}
