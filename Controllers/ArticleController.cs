using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("articles")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("drinks", Name = "GetDrinks")]
        public List<Artikel> GetAllDrinks()
        {
            List<Artikel> getraenke = new List<Artikel>();
            getraenke = DBAccess.GetAlleGetraenke();
            return getraenke;
        }

        [HttpGet("food", Name = "GetFood")]
        public List<Artikel> GetAllFood()
        {
            List<Artikel> speisen = new List<Artikel>();
            speisen = DBAccess.GetAlleSpeisen();
            return speisen;
        }

        [HttpGet("dessert", Name = "GetDesserts")]
        public List<Artikel> GetAllDesserts()
        {
            List<Artikel> desserts = new List<Artikel>();
            desserts = DBAccess.GetAlleDesserts();
            return desserts;
        }
    }
}


