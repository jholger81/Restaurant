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

        [HttpGet("income/today", Name = "GetDailyIncomeToday")]
        public int GetDailyIncomeToday()
        {
            DateTime day = DateTime.Today;
            int income;
            income = DBAccess.GetDailyIncome(day);

            return income;
        }

        [HttpGet("income/{day}", Name = "GetDailyIncome")]
        public int GetDailyIncome(string day)
        {
            var dayAsDT = Convert.ToDateTime(day);
            int income;
            income = DBAccess.GetDailyIncome(dayAsDT);

            return income;
        }

        [HttpGet("tips/today", Name = "GetDailyTipsToday")]
        public int GetDailyTipsToday()
        {
            DateTime day = DateTime.Today;
            int tips;
            tips = DBAccess.GetDailyTips(day);

            return tips;
        }

        [HttpGet("tips/{day}", Name = "GetDailyTips")]
        public int GetDailyTips(string day)
        {
            var dayAsDT = Convert.ToDateTime(day);
            int tips;
            tips = DBAccess.GetDailyTips(dayAsDT);

            return tips;
        }
    }
}
