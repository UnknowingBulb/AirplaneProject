using AiplaneProject.Objects;
using FluentResults;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneProject.Interactors
{
    [Authorize]
    public class OrderInteractor
    {/*
        private readonly OrderDbContext _OrderDbContext;
        public OrderInteractor(OrderDbContext OrderDbContext)
        {
            _OrderDbContext = OrderDbContext;
        }

        public Task Create(Order order)
        {
            _OrderDbContext.Order.Add(order);
            return _OrderDbContext.SaveChangesAsync();
        }

        public Task Delete(Guid orderId)
        {
            var order = new Order() { Id = orderId };
            _OrderDbContext.Order.Remove(order);
            return _OrderDbContext.SaveChangesAsync();
        }
        public Order GetOrder(Guid id) 
        {
        }

        public Order GetOrdersByPhone(string phoneNumber)
        {
        }

        public Order GetOrdersByUser(Guid id)
        {
        }

        private Result ValidateOrder(Order order)
        {
            return Result.Ok();
        }*/
    }
}
