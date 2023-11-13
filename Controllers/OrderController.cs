using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Net;

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
        public List<Bestellung> GetOrder(int? id_Tisch)
        {
            List<Bestellung> bestellung;
            try
            {
                bestellung = DBAccess.GetOrder(id_Tisch.Value);
            }
            catch (Exception ex)
            {
                bestellung = new List<Bestellung>();
            }

            return bestellung;
        }

        [HttpGet("{id_Tisch}/open", Name = "GetOrderOpenPositions")]
        public List<Bestellung> GetOrderOpenPositions(int? id_Tisch)
        {
            List<Bestellung> bestellungen;
            try
            {
                bestellungen = DBAccess.GetOrder(id_Tisch.Value, DBAccess.GetOrderMode.Open);
            }
            catch (Exception ex)
            {
                bestellungen = new List<Bestellung>();
            }

            return bestellungen;
        }

        [HttpGet("{id_Tisch}/closed", Name = "GetOrderClosedPositions")]
        public List<Bestellung> GetOrderClosedPositions(int? id_Tisch)
        {
            List<Bestellung> bestellung;
            try
            {
                bestellung = DBAccess.GetOrder(id_Tisch.Value, DBAccess.GetOrderMode.Closed);
            }
            catch (Exception ex)
            {
                bestellung = new List<Bestellung>();
            }

            return bestellung;
        }

        [HttpPost("new", Name = "PostNewOrder")]
        public HttpResponseMessage PostNewOrder([FromBody] Bestellung newOrder)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                DBAccess.InsertOrder(newOrder);
                result.StatusCode = HttpStatusCode.OK;
            }
            catch
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
          return result;
        }
    }
}


//TODO Aufruf Post etwa
//string jsonOrder = System.Text.Json.JsonSerializer.Serialize(newOrder);
//var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
//request.Content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
//HttpResponseMessage response = await httpClient.SendAsync(request);
//if (response.IsSuccessStatusCode)
//{
//    Console.WriteLine("Die Bestellung wurde erfolgreich an die API gesendet.");
//}
//else
//{
//    Console.WriteLine($"Fehler beim Senden der Bestellung. HTTP-Statuscode: {response.StatusCode}");
//}