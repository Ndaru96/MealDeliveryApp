using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<Order> GetOrders([Service] MealDeliveryContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole.Value == "BUYER")
                {
                    return context.Orders.Include(p => p.OrderDetails);
                }


            }


            return new List<Order>().AsQueryable();
        }
    }
}
