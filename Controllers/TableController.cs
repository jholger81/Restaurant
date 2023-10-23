﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("tables")]
    public class TableController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public TableController(ILogger<OrderController> logger)
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
    }
}
