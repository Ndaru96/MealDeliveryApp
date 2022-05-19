using HotChocolate.AspNetCore.Authorization;
using OrderService.Models;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<OrderData> AddOrderAsync(
           OrderInput input,
           [Service] MealAppContext context)
        {
            var meal = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (meal != null)
            {
                return await Task.FromResult(new OrderData());
            }
            var newOrder = new Order
            {
                Code = input.Code,
                UserId = input.UserId,
                CourierId = input.CourierId,
                StartDate = input.StartDate,
                Status = input.Status,
            };

            // EF
            var ret = context.Orders.Add(newOrder);
            await context.SaveChangesAsync();

            return await Task.FromResult(new OrderData
            {
                Id = newOrder.Id,
                UserId = newOrder.UserId,
                CourierId = newOrder.CourierId,
                StartDate = newOrder.StartDate,
                EndDate = newOrder.EndDate,
                Status = newOrder.Status,
            });
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<OrderDetailData> AddOrderDetailAsync(
           OrderDetailInput input,
           [Service] MealAppContext context)
        {
            var orderDetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
            if (orderDetail != null)
            {
                return await Task.FromResult(new OrderDetailData());
            }
            var newOrderDetail = new OrderDetail
            {
                MealId = input.MealId,
                OrderId = input.OrderId,
                Quantity = input.Quantity
            };

            // EF
            var ret = context.OrderDetails.Add(newOrderDetail);
            await context.SaveChangesAsync();

            return await Task.FromResult(new OrderDetailData
            {
                Id = newOrderDetail.Id,
                MealId = newOrderDetail.MealId,
                OrderId = newOrderDetail.OrderId,
                Quantity = newOrderDetail.Quantity
            });
        }
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Order> UpdateOrderAsync(
           UpdateOrder input,
           [Service] MealAppContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                order.Code = input.Code;
                order.UserId = input.UserId;
                order.CourierId = input.CourierId;
                order.StartDate = input.StartDate;
                order.EndDate = input.EndDate;
                order.Status = input.Status;

                context.Orders.Update(order);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(order);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<OrderDetail> UpdateOrderDetailAsync(
          UpdateOrderDetail input,
          [Service] MealAppContext context)
        {
            var orderdetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
            if (orderdetail != null)
            {
                orderdetail.MealId = input.MealId;
                orderdetail.OrderId = input.OrderId;
                orderdetail.Quantity = input.Quantity;
               

                context.OrderDetails.Update(orderdetail);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(orderdetail);
        }
    }
}
