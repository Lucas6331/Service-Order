using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Dtos.ServiceOrders;

namespace OsDsII.api.Services.ServiceOrders
{
    public interface IServiceOrderService
    {
        public Task<NewServiceOrderDto> CreateAsync(CreateServiceOrderDto serviceOrder);
        public Task<List<ServiceOrderDto>> GetAllAsync();
        public Task<ServiceOrderDto> GetByIdAsync(int id);
        public Task FinishAsync(int id);
        public Task CancelAsync(int id);

    }
}
