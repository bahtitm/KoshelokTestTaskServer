using AutoMapper;
using Domain.Entities;

namespace Application.Features.Messages
{
    internal class MessageAutoMapperProfile : Profile
    {
        public MessageAutoMapperProfile()
        {

            CreateMap<CreateMessageRequest, Message>();


            CreateMap<UpdateMessageRequest, Message>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore both null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;



                        return true;
                    }
                ));
        }
    }
}
