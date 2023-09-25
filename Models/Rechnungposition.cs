using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Rechnungposition
    {
        public int ID_Rechnungposition { get; set; }
        public int ID_Artikel { get; set; }
        public int ID_Rechnung { get; set; }
        public Artikel Artikel { get; set; }
    }
}
