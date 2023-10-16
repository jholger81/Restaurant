using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Rechnungspositionen
    /// <para>Attribute:</para>
    /// <para>int ID_Rechnungsposition</para>
    /// <para>int ID_Artikel</para>
    /// <para>int ID_Rechnung</para>
    /// <para>Artikel Artikel</para>
    /// </summary>
    public class Rechnungposition
    {
        public int ID_Rechnungposition { get; set; }
        public int ID_Artikel { get; set; }
        public int ID_Rechnung { get; set; }
        public Artikel Artikel { get; set; }
    }
}
