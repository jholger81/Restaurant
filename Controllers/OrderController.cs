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
            Bestellung myOrder = new Bestellung();
            myOrder.ID_Bestellung = 123;

            //return myOrder;
            return "123";
        }

        [HttpPost(Name = "PostNewOrder")]
        public void PostNewOrder(Bestellung newOrder)
        {
            newOrder = new Bestellung();
            newOrder.OrderDate = DateTime.Now;
            newOrder.TableNumber = 1;
            newOrder.OrderList = new List<Artikel>(); // TODO

        }
    }
}