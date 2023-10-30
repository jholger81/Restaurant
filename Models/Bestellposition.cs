namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Bestellpositionen
    /// <para>Attribute:</para>
    /// <para>int ID_Bestellposition</para>
    /// <para>int ID_Artikel</para>
    /// <para>int ID_Bestellung</para>
    /// <para>atring Extras</para>
    /// <para>int Geliefert</para>
    /// <para>Artikel Artikel</para>
    /// </summary>
    public class Bestellposition
    {
        public enum PositionsStatus
        {
            Bestellt = 0,
            Geliefert = 1,
            Bezahlt = 2
        }

        public int ID_Bestellposition { get; set; }
        public int ID_Artikel { get; set; }
        public int ID_Bestellung { get; set; }
        public string Extras { get; set; }
        public PositionsStatus Geliefert { get; set; }
        public Artikel Artikel { get; set; }
    }
}
