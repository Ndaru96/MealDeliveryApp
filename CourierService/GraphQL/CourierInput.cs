namespace CourierService.GraphQL
{
    public record CourierInput
        (
            int? Id,
            string Name,
            string Phone,
            int UserId
        );
    
}
