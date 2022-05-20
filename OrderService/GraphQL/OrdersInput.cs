using OrderService.Models;

namespace OrderService.GraphQL
{
    public record OrdersInput
        (
            int? Id,
            string Code,
            int? CourierId,
            string? Longitude,
            string? Latitude,
            List<OrderDetail> OrderDetails
            
        );
    
}
