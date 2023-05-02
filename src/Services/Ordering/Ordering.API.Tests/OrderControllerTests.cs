using NUnit.Framework;
using Moq;
using MediatR;
using Ordering.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Threading;
using Ordering.API.Tests.Mocks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.Logging;
using Polly;

namespace Ordering.API.Tests
{
    public class OrderControllerTests
    {
        //private Mock<IMediator> _mediator;
        //private OrderController _controller;

        //[SetUp]
        //public void Setup()
        //{
        //    _mediator = new Mock<IMediator>();
        //    _mediator.Setup(p => p.Send(It.IsAny<GetOrdersListQuery>(), default(CancellationToken))).ReturnsAsync(MockOrderRepository.GetListOrderVM());
        //    _controller = new OrderController(_mediator.Object);
        //}

        //[Test]
        //public void GetOrdersByUserName_ShouldReturnAtLeastOneOrder()
        //{
        //    //act
        //    var result = _controller.GetOrdersByUserName("swn");

        //    Assert.Pass();
        //}

        [TestFixture]
        public class MyEndpointTests
        {
            private WebApplicationFactory<Startup> _factory;
            private HttpClient _client;
            private static DbContextOptions<OrderContext> dbContextOptions = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName:"OrderDb")
                .Options;

                OrderContext contextDb;

            [OneTimeSetUp]
            public void OneTimeSetup()
            {
                contextDb = new OrderContext(dbContextOptions);
                contextDb.Database.EnsureCreated();

                var logger = new Mock<ILogger<OrderContextSeed>>();

                OrderContextSeed
                    .SeedAsync(contextDb, logger.Object)
                    .Wait();

                //var mockQueryHandler = new Mock<IRequestHandler<GetOrdersListQuery, List<OrdersVm>>>();
                //mockQueryHandler.Setup(h => h.Handle(It.IsAny<GetOrdersListQuery>(), default))
                //    .ReturnsAsync(MockOrderRepository.GetListOrderVM());

                //_factory = new WebApplicationFactory<Startup>()
                //    .WithWebHostBuilder(builder =>
                //    {
                //        builder.ConfigureServices(services =>
                //        {
                //            //services.AddTransient(typeof(IRequestHandler<GetOrdersListQuery, List<OrdersVm>>), _ => mockQueryHandler.Object);
                //        });
                //    });

                //_client = _factory.CreateClient();
            }

            [OneTimeTearDown]
            public void cleanUp()
            {
                contextDb.Database.EnsureDeleted();
            }

            //[SetUp]
            //public void Setup()
            //{
               
            //}

            [Test]
            public async Task TestGetMyDataEndpoint()
            {
                // Arrange
                //var expectedData = MockOrderRepository.GetListOrderVM();
                var mediator= new Mock<IMediator>();
                var mockQueryHandler = new Mock<IRequestHandler<GetOrdersListQuery, List<OrdersVm>>>();
                mockQueryHandler.Setup(h => h.Handle(It.IsAny<GetOrdersListQuery>(), default))
                    .ReturnsAsync(MockOrderRepository.GetListOrderVM());
                var controller = new OrderController(mediator.Object);

                // Act
                var response = await _client.GetAsync("/api/v1/order/swn");
                response.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<List<OrdersVm>> (await response.Content.ReadAsStringAsync());

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                //Assert.AreEqual(expectedData.Id, responseData.Id);
                //Assert.AreEqual(expectedData.Name, responseData.Name);
            }

            [TearDown]
            public void Teardown()
            {
                _client.Dispose();
                _factory.Dispose();
            }
        }
    }
}