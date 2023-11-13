using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("tables")]
    public class TableController : ControllerBase
    {
        private readonly ILogger<TableController> _logger;

        public TableController(ILogger<TableController> logger)
        {
            _logger = logger;
        }

        [HttpGet("", Name = "GetAllTables")]
        public List<Tisch> GetAllTables(int? id_Tisch)
        {
            List<Tisch> tische = new List<Tisch>();
            try
            {
                tische = DBAccess.GetAlleTische();
            }
            catch (Exception ex)
            {
                tische = new List<Tisch>();
            }

            return tische;
        }

        [HttpGet("open", Name = "GetAllTablesWithOpenOrders")]
        public List<Tisch> GetAllTablesWithOpenOrders()
        {
            List<Tisch> tische = new List<Tisch>();
            try
            {
                tische = DBAccess.GetTablesWithOpenOrders();
            }
            catch (Exception ex)
            {
                tische = new List<Tisch>();
            }

            return tische;
        }

        [HttpPut("switch/{vonTisch}/{zuTisch}", Name = "SwitchTable")]
        public HttpResponseMessage SwitchTables(int vonTisch, int zuTisch)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                DBAccess.SwitchTables(vonTisch, zuTisch);
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
