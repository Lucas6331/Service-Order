using OsDsII.api.Models;

namespace OsDsII.api.Repository.ServiceOrders
{
    // Dependency inversion principle (solid)
    public interface IServiceOrderRepository
    {
        public Task<List<ServiceOrder>> GetAllAsync();
        public Task<ServiceOrder> GetByIdAsync(int id);
        public Task<ServiceOrder> GetServiceOrderWithCommentsAsync(int serviceOrderId);
        public Task<ServiceOrder> GetServiceOrderFromCustomerAsync(int serviceOrderId);
        public Task AddAsync(ServiceOrder serviceOrder);
        public Task FinishAsync(ServiceOrder serviceOrder);
        public Task CancelAsync(ServiceOrder serviceOrder);
    }
}
