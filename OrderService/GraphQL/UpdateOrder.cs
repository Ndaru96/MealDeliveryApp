namespace OrderService.GraphQL
{
    public record UpdateOrder
        (
            int? Id,
            string Code,
            int UserId,
            int CourierId,
            DateTime StartDate,
            DateTime EndDate,
            bool Status
        );
    
}
