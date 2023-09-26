using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Beziehung zwischen Bestellungen und Rechnungen
    /// <para>Attribute:</para>
    /// <para>int ID_Bestellung</para>
    /// <para>int ID_Rechnung</para>
    /// </summary>
    public class Bestellung_Rechnung
    {
        public int ID_Bestellung { get; set; }
        public int ID_Rechnung { get; set; }
    }
}
