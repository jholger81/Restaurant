namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Artikel
    /// <para>Attribute:</para>
    /// <para>int ID_Artikel</para>
    /// <para>string Name</para>
    /// <para>int Preis (in Cent)</para>
    /// </summary>
    public class Artikel
    {
        public int ID_Artikel { get; set; }
        public string Name { get; set; }
        public int Preis { get; set; }
        public string Kategorie { get; set; }
    }
}
