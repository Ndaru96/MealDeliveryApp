namespace UserService.GraphQL
{
    public record ProfileInput
    (
        
        int UserId,
        string Name,
        string Address,
        string City,
        string Phone
        );
    
}
