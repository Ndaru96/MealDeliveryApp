using HotChocolate.AspNetCore.Authorization;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<Order> AddOrderAsync(
           OrdersInput input,
           [Service] MealDeliveryContext context, ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var transaction = context.Database.BeginTransaction();
                try
                {
                    var newOrder = new Order
                    {
                        Code = input.Code,
                        UserId = user.Id,
                        Longitude = input.Longitude,
                        Latitude = input.Latitude,
                        Status = false

                    };
                    for (int i = 0; i < input.OrderDetails.Count; i++)
                    {
                        var orderdetail = new OrderDetail
                        {
                            OrderId = newOrder.Id,
                            MealId = input.OrderDetails[i].MealId,
                            Quantity = input.OrderDetails[i].Quantity
                        };
                        newOrder.OrderDetails.Add(orderdetail);
                    }
                    // EF
                    var ret = context.Orders.Add(newOrder);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return ret.Entity;
                }
                catch
                {
                    transaction.Rollback();
                }


            }

            return new Order();



        }



        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Order> AddCourierAsync(
           UpdateOrder input,
           [Service] MealDeliveryContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            var courier = context.Couriers.Where(o => o.Id == input.CourierId).FirstOrDefault();
            if (order != null)
            {
                if (courier.Status == false)
                {
                    var transaction = context.Database.BeginTransaction();
                    try
                    {
                        courier.Status = true;
                        order.CourierId = input.CourierId;

                        var ret = context.Orders.Update(order);
                        await context.SaveChangesAsync();
                        transaction.Commit();

                        return ret.Entity;

                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return new Order();
        }
            

        [Authorize(Roles = new[] { "COURIER" })]
        public async Task<Order> UpdateOrderAsync(
            LatesOrder input,
            [Service] MealDeliveryContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();

            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();

            var courier = context.Couriers.Where(o => o.UserId == user.Id).FirstOrDefault();

            if (order != null && order.CourierId == courier.Id)
            {
                if (input.Status == true)
                {
                    var transaction = context.Database.BeginTransaction();
                    try
                    {
                        order.Courier.Status = false;
                        order.Status = input.Status;
                        

                        var ret = context.Orders.Update(order);
                        await context.SaveChangesAsync();
                        transaction.Commit();

                        return ret.Entity;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return new Order { CourierId = null, Code = "Null", Id = 0, UserId = 0 };
        }

        //[Authorize(Roles = new[] { "MANAGER" })]
        //public async Task<Order> DeleteOrderAsync(
        //    int id,
        //    [Service] MealDeliveryContext context)
        //{
        //    var order = context.Orders.Where(o => o.Id == id).Include(n => n.OrderDetails).FirstOrDefault();
        //    if (order != null && order.Status == true)
        //    {
        //        //context.Orders.Remove(order);
        //        //await context.SaveChangesAsync();
        //        return new Order { CourierId = null, Code = "Terhapus", Id = 0, UserId = 0 };
        //    }
        //    else
        //    {
        //        return new Order { CourierId = null, Code = "Null", Id = 0, UserId = 0 };
        //    }
        //    return order;
        //}
    }
}
