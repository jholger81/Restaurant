using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("waiter")]
    public class WaiterController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public WaiterController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id_Tisch}", Name = "GetTablesForWaiter")]
        public List<Tisch> GetTablesForWaiter(int? id_Tisch)
        {
            //TODO

            List<Tisch> tischliste;
            try
            {
                tischliste = DBAccess.GetTablesForWaiter(id_Tisch.Value);
            }
            catch (Exception ex)
            {
                tischliste = new List<Tisch>();
            }

            //return bestellung;


            return new List<Tisch>();
        }

        [HttpPut(Name = "PutTableForWaiter")]
        public void PutTableForWaiter(int id_Tisch)
        {
            // TODO
        }
    }
}
