using System.Collections.Generic;
using System;

namespace Restaurant.Models
{
    public class Bestellung
    {
        public int ID_Bestellung { get; set; }
        public DateTime Datum { get; set; }
        public int Gäste { get; set; }
        public int ID_Tisch { get; set; }
        public int Einnahmen { get; set; }
        public List<Bestellposition> Positionen { get; set; }
    }
}
