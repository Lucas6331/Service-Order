using AutoMapper;
using OsDsII.api.Dtos.Comments;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Models;

namespace OsDsII.api
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();

            CreateMap<ServiceOrder, CreateServiceOrderDto>();
            CreateMap<CreateServiceOrderDto, ServiceOrder>();
            CreateMap<ServiceOrder, NewServiceOrderDto>();
            CreateMap<ServiceOrder, ServiceOrderDto>();

            CreateMap<Comment, CreateCommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<Comment, NewCommentDto>();
            CreateMap<Comment, CommentDto>();
        }
    }
}
