namespace OrderService.GraphQL
{
    public record UpdateOrder
        (
            int? Id,
            string Code,
            int UserId,
            int CourierId,
            string? Longitude,
            string? Latitude
            
        );
    
}
