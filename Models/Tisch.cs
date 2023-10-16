namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Tische
    /// <para>Attribute:</para>
    /// <para>int ID_Tisch</para>
    /// <para>int ID_Kellner</para>
    /// </summary>
    public class Tisch
    {
        public int ID_Tisch { get; set; }
        public int ID_Kellner { get; set; }
    }
}
