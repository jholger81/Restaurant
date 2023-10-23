namespace Restaurant.Models
{
    /// <summary>
    /// Modell für Kellner
    /// <para>Attribute:</para>
    /// <para>int ID_Kellner</para>
    /// <para>string Vorname</para>
    /// <para>string Nachname</para>
    /// </summary>
    public class Kellner
    {
        public int ID_Kellner { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
    }
}
