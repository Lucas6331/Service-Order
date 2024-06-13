using OsDsII.api.Dtos.ServiceOrders;
using OsDsII.api.Models;

namespace OsDsII.api.Dtos.Customers
{
    public record CustomerDto(string Name, string Email, string Phone, List<ServiceOrderDto> ListServiceOrder)
    {
        public CustomerDto() : this("", "", "", null) { }
    }
}
