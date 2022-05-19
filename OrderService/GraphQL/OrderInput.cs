namespace OrderService.GraphQL
{
    public record OrderInput
        (
            int? Id,
            string Code,
            int UserId,
            int CourierId,
            DateTime? StartDate,
            DateTime? EndDate,
            bool Status
        );
    
}
