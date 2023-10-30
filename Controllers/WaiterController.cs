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

        [HttpGet("{id_Kellner}", Name = "GetTablesForWaiter")]
        public List<Tisch> GetTablesForWaiter(int id_Kellner)
        {
            List<Tisch> tischliste;
            try
            {
                tischliste = DBAccess.GetTischeForKellner(id_Kellner);
            }
            catch (Exception ex)
            {
                tischliste = new List<Tisch>();
            }
            return tischliste;
        }

        [HttpPut("switch/{id_Kellner}/{id_Tisch}", Name = "PutTableForWaiter")]
        public void SwitchWaiterTableForTable(int id_Kellner, int id_Tisch)
        {
            DBAccess.SwitchWaiterTableForTable(id_Kellner, id_Tisch);
        }
    }
}
