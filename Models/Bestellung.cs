using System.Collections.Generic;
using System;

namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Bestellungen
    /// <para>Attribute:</para>
    /// <para>int ID_Bestellung</para>
    /// <para>DateTime Datum</para>
    /// <para>int ID_Tisch</para>
    /// <para>List(Bestellposiiton) Positionen</para>
    /// </summary>
    public class Bestellung
    {
        public int ID_Bestellung { get; set; }
        public DateTime Datum { get; set; }
        public int ID_Tisch { get; set; }
        public List<Bestellposition> Positionen { get; set; }
    }
}
