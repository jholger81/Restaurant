using Microsoft.AspNetCore.Mvc;
using Restaurant.Database;
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

        [HttpGet("{id_Tisch}", Name = "GetOrder")]
        public Bestellung GetOrder(int? id_Tisch)
        {
            Bestellung bestellung;
            try
            {
                bestellung = DBAccess.GetOrder(id_Tisch.Value);
            }
            catch (Exception ex)
            {
                bestellung = new Bestellung();
            }
            
            return bestellung;
        }

        [HttpPost(Name = "PostNewOrder")]
        public void PostNewOrder(Bestellung newOrder)
        {
            newOrder = new Bestellung();
            newOrder.Datum = DateTime.Now;
            newOrder.ID_Tisch = 1;
            newOrder.Positionen = new List<Bestellposition>(); // TODO
        }
    }
}