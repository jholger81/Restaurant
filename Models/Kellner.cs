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
        int ID_Kellner { get; set; }
        string Vorname { get; set; }
        string Nachname { get; set; }
    }
}
