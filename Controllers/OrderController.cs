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

        [HttpGet("open", Name = "GetAllOpenOrderPositions")]
        public List<Bestellposition> GetAllOpenOrderPositions()
        {
            List<Bestellposition> positions;
            try
            {
                positions = DBAccess.GetAllOpenOrderPositions();
            }
            catch (Exception ex)
            {
                positions = new List<Bestellposition>();
            }

            return positions;
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

        [HttpPost("pay/{trinkgeld}", Name = "PayOrder")]
        public HttpResponseMessage PayOrder([FromBody] List<Bestellposition> orderpositions, int trinkgeld)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                var reconstructedPositions = DBAccess.ReconstructPositions(orderpositions);
                DBAccess.PayBillPartially(reconstructedPositions, trinkgeld);
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