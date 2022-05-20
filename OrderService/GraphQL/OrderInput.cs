namespace OrderService.GraphQL
{
    public record OrderInput
        (
            int? Id,
            string Code,
            int UserId,
            int CourierId,
            string? Longitude,
            string? Latitude
            
        );
    
}
