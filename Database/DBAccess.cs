using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Data.SQLite;

namespace Restaurant.Database
{
    public static class DBAccess
    {
        public static Bestellung GetOrder(int id_Tisch)
        {
            Bestellung bestellung = new Bestellung();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Bestellung, Datum, ID_Tisch FROM Bestellung WHERE ID_Tisch = {id_Tisch}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                bestellung.ID_Bestellung = sqlitereader.GetInt32(0);
                bestellung.Datum = DateTime.Parse(sqlitereader.GetString(1));
                bestellung.ID_Tisch = sqlitereader.GetInt32(2);
            }
            sqliteconnection.Close();

            bestellung.Positionen = GetOrderPositions(bestellung.ID_Bestellung);

            foreach (Bestellposition position in bestellung.Positionen)
            {
                position.Artikel = GetArticle(position.ID_Artikel);
            }

            return bestellung;
        }

        public static void InsertOrder()
        {
            // TODO
        }

        public static void UpdateOrder()
        {
            // TODO
        }

        public static Artikel GetArticle(int id_Artikel)
        {
            Artikel artikel = new Artikel();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Artikel, Name, Preis FROM Artikel WHERE ID_Artikel = {id_Artikel}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                artikel.ID_Artikel = sqlitereader.GetInt32(0);
                artikel.Name = sqlitereader.GetString(1);
                artikel.Preis = sqlitereader.GetInt32(2);
            }
            sqliteconnection.Close();

            return artikel;
        }

        public static Kellner GetKellner()
        {
            // TODO
            return new Kellner();
        }

        public static Tisch GetTisch()
        {
            // TODO
            return new Tisch();
        }

        public static List<Bestellposition> GetOrderPositions(int id_Bestellung)
        {
            List<Bestellposition> positionen = new List<Bestellposition>();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Bestellposition, ID_Artikel, ID_Bestellung, Extras, Geliefert FROM Bestellposition WHERE ID_Bestellung = {id_Bestellung}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                Bestellposition position = new Bestellposition();
                position.ID_Bestellposition = sqlitereader.GetInt32(0);
                position.ID_Artikel = sqlitereader.GetInt32(1);
                position.ID_Bestellung = sqlitereader.GetInt32(2);
                try
                {
                    position.Extras = sqlitereader.GetString(3);
                }
                catch (Exception ex)
                {
                    position.Extras = null;
                    Console.WriteLine($"{ex.Message}");
                }
                
                position.Geliefert = sqlitereader.GetInt32(4);
                positionen.Add(position);
            }
            sqliteconnection.Close();

            return positionen;
        }

        internal static List<Tisch> GetTablesForWaiter(int id_Kellner)
        {
            List<Tisch> tischliste = new List<Tisch> ();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Tisch, FROM Tisch WHERE ID_Kellner = {id_Kellner}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                Tisch tisch = new Tisch();
                tisch.ID_Tisch = sqlitereader.GetInt32(0);
                tisch.ID_Kellner = id_Kellner;
                tischliste.Add(tisch);
            }
            sqliteconnection.Close();

            return tischliste;
        }
    }
}
