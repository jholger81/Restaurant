using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;

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

        [HttpGet("{id_Tisch}/open", Name = "GetOrderOpenPositions")]
        public Bestellung GetOrderOpenPositions(int? id_Tisch)
        {
            Bestellung bestellung;
            try
            {
                bestellung = DBAccess.GetOrder(id_Tisch.Value, DBAccess.GetOrderMode.Open);
            }
            catch (Exception ex)
            {
                bestellung = new Bestellung();
            }

            return bestellung;
        }

        [HttpGet("{id_Tisch}/closed", Name = "GetOrderClosedPositions")]
        public Bestellung GetOrderClosedPositions(int? id_Tisch)
        {
            Bestellung bestellung;
            try
            {
                bestellung = DBAccess.GetOrder(id_Tisch.Value, DBAccess.GetOrderMode.Closed);
            }
            catch (Exception ex)
            {
                bestellung = new Bestellung();
            }

            return bestellung;
        }

        [HttpPost("new", Name = "PostNewOrder")]
        public void PostNewOrder([FromBody] Bestellung newOrder)
        {
            DBAccess.InsertOrder(newOrder);
        }
    }
}


//TODO Aufruf Post etwa
//string jsonOrder = System.Text.Json.JsonSerializer.Serialize(newOrder);
//var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
//request.Content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");