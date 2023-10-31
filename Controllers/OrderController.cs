using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;

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

        [HttpPost(Name = "PostNewOrder")]
        public void PostNewOrder()//(Bestellung newOrder)
        {
            Bestellung newOrder = new Bestellung();
            newOrder = new Bestellung();
            newOrder.Datum = DateTime.Now;
            newOrder.ID_Tisch = 1;
            newOrder.Positionen = new List<Bestellposition>(); // TODO        

            Bestellposition newPos = new Bestellposition();
            newPos.Geliefert = 0;
            newPos.ID_Artikel = 1;
            newPos.Extras = "keine Tomaten plx";
            newOrder.Positionen.Add(newPos);

            Bestellposition newPos2 = new Bestellposition();
            newPos.Geliefert = 0;
            newPos.ID_Artikel = 2;
            newPos.Extras = "";
            newOrder.Positionen.Add(newPos2);
            DBAccess.InsertOrder(newOrder);
        }
    }
}