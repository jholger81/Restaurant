using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Rechnungen
    /// <para>Attribute:</para>
    /// <para>int ID_Bestellung</para>
    /// <para>int Trinkgeld (in Cent)</para>
    /// </summary>
    public class Rechnung
    {
        public int ID_Bestellung { get; set; }
        int Trinkgeld { get; set; }
        public List<Rechnungposition> Positionen { get; set; }
    }
}
