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
    [Route("waiter")]
    public class WaiterController : ControllerBase
    {
        private readonly ILogger<WaiterController> _logger;

        public WaiterController(ILogger<WaiterController> logger)
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

        [HttpGet("login", Name = "Login")]
        public Kellner Login([FromBody]Kellner kellner)
        {
            int kellnerID;
            Kellner kellnerObj = new Kellner();
            try
            {
                kellnerID = DBAccess.CheckLogin(kellner);
            }
            catch (Exception ex)
            {
                kellnerID = 0;
            }
            kellnerObj.ID_Kellner = kellnerID;
            return kellnerObj;
        }

        [HttpPut("switch/{id_Kellner}/{id_Tisch}", Name = "PutTableForWaiter")]
        public HttpResponseMessage SwitchWaiterTableForTable(int id_Kellner, int id_Tisch)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                DBAccess.SwitchWaiterForTable(id_Kellner, id_Tisch);
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
