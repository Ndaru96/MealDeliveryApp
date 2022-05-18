namespace CourierService.GraphQL
{
    public record UpdateCourier
        (
            int? Id,
            string Name,
            string Phone,
            int UserId

        );
    
}
