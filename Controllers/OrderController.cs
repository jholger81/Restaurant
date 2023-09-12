using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetOrder")]
        public string GetOrder(int OrderNumber)
        {
            //TODO
            Order myOrder = new Order();
            myOrder.OrderNumber = 123;

            //return myOrder;
            return "123";
        }
    }
}