using Moq;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.API.Tests.Mocks
{
    public static class MockOrderRepository
    {
        public static Mock<IOrderRepository> GetOrderRepository()
        {
            List<Order> orders = GetListOrder();
            
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(p => p.GetOrdersByUserName(It.IsAny<string>())).ReturnsAsync(orders);
            return mockRepo;
        }

        public static List<Order> GetListOrder()
        {
            return new List<Order>()
            {
             new Order {UserName = "X87441", FirstName = "Pierre-André", LastName="Dejoie"}
            };
        }

        public static List<OrdersVm> GetListOrderVM()
        {
            return new List<OrdersVm>()
            {
             new OrdersVm {UserName = "X87441", FirstName = "Pierre-André", LastName="Dejoie"}
            };
        }
    }
}
