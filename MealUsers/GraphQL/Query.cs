using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Models;

namespace UserService.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<User> GetUsers([Service] MealAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "ADMIN")
                {
                    return context.Users.Include(p => p.Profiles);
                }
                var orders = context.Users.Where(o => o.Id == user.Id);
                return orders.AsQueryable();

            }


            return new List<User>().AsQueryable();
        }
    }
}
