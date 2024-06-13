using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Dtos.Comments;
using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Models;

namespace OsDsII.api.Services.Comments
{
    public interface ICommentService
    {
        public Task<ServiceOrderDto> GetByIdAsync(int serviceOrderId);
        public Task<NewCommentDto> AddAsync(int serviceOrderId, CreateCommentDto createCommentDto);

    }
}