using OsDsII.api.Models;

namespace OsDsII.api.Dtos.ServiceOrders
{
    public record NewServiceOrderDto (string Description, double Price, StatusServiceOrder Status, DateTimeOffset OpeningDate, DateTimeOffset? FinishDate, int CustomerId)
    { }
}
