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
    [Route("welcome")]
    public class WelcomeController : ControllerBase
    {
        private readonly ILogger<WelcomeController> _logger;

        public WelcomeController(ILogger<WelcomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("", Name = "Welcome")]
        public string Welcome()
        {
            return "Server running...";
        }
    }
}
