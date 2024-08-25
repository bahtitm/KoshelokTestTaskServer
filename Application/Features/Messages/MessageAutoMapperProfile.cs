using AutoMapper;
using Domain.Entities;

namespace Application.Features.Messages
{
    internal class MessageAutoMapperProfile : Profile
    {
        public MessageAutoMapperProfile()
        {

            CreateMap<CreateMessageRequest, Message>()
            //.BeforeMap((sr, ds) =>
            //{

            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        sr.Text.CopyToAsync(ms);
            //        ds.Text = ms.ToArray();

            //    }


            //})
            //.ForMember(p=>p.Text,p=>p.Ignore())
            ;


            CreateMap<UpdateMessageRequest, Message>()
                  .BeforeMap((sr, ds) =>

                  {

                      using (MemoryStream ms = new MemoryStream())
                      {
                          sr.Text.CopyToAsync(ms);
                          ds.Text = ms.ToArray();

                      }


                  })
                  .ForMember(p => p.Text, p => p.Ignore())

            ;

            CreateMap<Message, GetedMessage>()
                  .BeforeMap((sr, ds) =>

                  {
                      string result = System.Text.Encoding.UTF8.GetString(sr.Text);


                      ds.Text = result;




                  })
                  .ForMember(p => p.Text, p => p.Ignore())

            ;

        }
    }
}
