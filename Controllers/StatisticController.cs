using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Database;
using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly ILogger<StatisticController> _logger;

        public StatisticController(ILogger<StatisticController> logger)
        {
            _logger = logger;
        }

        [HttpGet("income", Name = "GetDailyIncome")]
        public int GetDailyIncome(DateTime day = default(DateTime))
        {
            int income;
            if (day == default(DateTime))
                day = DateTime.Today;
            income = DBAccess.GetDailyIncome(day);

            return income;
        }

        [HttpGet("tips", Name = "GetDailyTips")]
        public int GetDailyTips(DateTime day = default(DateTime))
        {
            int tips;
            if (day == default(DateTime))
                day = DateTime.Today;
            tips = DBAccess.GetDailyTips(day);

            return tips;
        }
    }
}
