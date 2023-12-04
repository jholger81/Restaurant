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
    [Route("invoice")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public InvoiceController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("fromTable/{id_Tisch}", Name = "GetPaidInvoicePositionsForOrder")]
        public List<Rechnungposition> GetPaidInvoicePositionsForTable(int id_Tisch)
        {
            List<Rechnungposition> rechnungsPositionen;
            try
            {
                rechnungsPositionen = DBAccess.GetPaidPositions(id_Tisch);
            }
            catch (Exception ex)
            {
                rechnungsPositionen = new List<Rechnungposition>();
            }

            return rechnungsPositionen;
        }
    }
}