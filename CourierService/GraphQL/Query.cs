using CourierService.Models;
using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourierService.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<Courier> GetCourier([Service] MealAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "MANAGER")
                {
                    return context.Couriers;
                }


            }


            return new List<Courier>().AsQueryable();
        }
    }
}
