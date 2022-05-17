namespace UserService.GraphQL
{
    public record ChangePassword
        (
            int Id,
            string username,
            string password
        );
    
}
