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

        public static void InsertOrder(Bestellung neueBestellung)
        {
            string sqlInsertOrderPos;
            int idBestellung;
            int idBestellPos;
            string strTemp = "Data Source=Database.db3";
            string bestellzeitpunkt = neueBestellung.Datum.ToString("yyyy-MM-dd hh:mm:ss");
            string sqlInsertOrder = $"INSERT INTO Bestellung(Datum, ID_Tisch) VALUES('{bestellzeitpunkt}',{neueBestellung.ID_Tisch}) RETURNING ID_Bestellung";

            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertOrder, sqliteconnection);
            var idBestellungObj = sqlitecommand.ExecuteScalar();
            idBestellung = Convert.ToInt32(idBestellungObj);
            neueBestellung.ID_Bestellung = (int)idBestellung;

            foreach (Bestellposition position in neueBestellung.Positionen)
            {
                sqlInsertOrderPos = $"INSERT INTO Bestellposition(ID_Artikel, ID_Bestellung, Extras, Geliefert) VALUES({position.ID_Artikel}, {idBestellung}, '{position.Extras}', {position.Geliefert}) RETURNING ID_Bestellposition";
                sqlitecommand.CommandText = sqlInsertOrderPos;
                var bestellPosIDObj = sqlitecommand.ExecuteScalar();
                idBestellPos = Convert.ToInt32(bestellPosIDObj);
                position.ID_Bestellposition = idBestellPos;
            }
        }

           
        public static void UpdateOrder(Bestellung neueBestellung)
        {
            string sqlUpdateOrderPos;
            int idBestellung;
            int idBestellPos;
            string strTemp = "Data Source=Database.db3";
            string bestellzeitpunkt = neueBestellung.Datum.ToString("yyyy-MM-dd hh:mm:ss");
            string sqlInsertOrder = $"UPDATE Bestellung SET Datum = '{bestellzeitpunkt}', ID_Tisch = {neueBestellung.ID_Tisch}) WHERE ID_Bestellung = {neueBestellung.ID_Bestellung}";

            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertOrder, sqliteconnection);
            sqlitecommand.ExecuteNonQuery();

            foreach (Bestellposition position in neueBestellung.Positionen)
            {
                sqlUpdateOrderPos = $"UPDATE Bestellposition SET ID_Artikel = {position.ID_Artikel}, Extras = {position.Extras}, Geliefert = {position.Geliefert} WHERE ID_Position = {position.ID_Bestellposition}";
                sqlitecommand.CommandText = sqlUpdateOrderPos;
                var bestellPosIDObj = sqlitecommand.ExecuteScalar();
                idBestellPos = Convert.ToInt32(bestellPosIDObj);
                position.ID_Bestellposition = idBestellPos;
            }
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

        public static Kellner GetKellner(int id_Kellner)
        {
            Kellner kellner = new Kellner();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Kellner, Nachname, Vorname FROM Kellner WHERE ID_Kellner = {id_Kellner}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                kellner.ID_Kellner = sqlitereader.GetInt32(0);
                kellner.Nachname = sqlitereader.GetString(1);
                kellner.Vorname = sqlitereader.GetString(2);
            }
            sqliteconnection.Close();
            return kellner;
        }

        public static Kellner GetKellner(string nachname, string vorname = null)
        {
            Kellner kellner = new Kellner();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Kellner, Nachname, Vorname FROM Kellner WHERE Nachname = '{nachname}'";
            if (vorname != null)
                sql += $" AND Vorname = '{vorname}'";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                kellner.ID_Kellner = sqlitereader.GetInt32(0);
                kellner.Nachname = sqlitereader.GetString(1);
                kellner.Vorname = sqlitereader.GetString(2);
            }
            sqliteconnection.Close();
            return kellner;
        }

        public static Tisch GetTisch(int id_Tisch)
        {
            Tisch tisch = new Tisch();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Tisch, ID_Kellner FROM Tisch WHERE ID_Tisch = {id_Tisch}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                tisch.ID_Tisch = sqlitereader.GetInt32(0);
                tisch.ID_Kellner = sqlitereader.GetInt32(1);
            }
            sqliteconnection.Close();
            return tisch;
        }

        public static List<Tisch> GetTischeForKellner(int id_Kellner)
        {
            List<Tisch> tische = new List<Tisch>();

            string strTemp = "Data Source=Database.db3";
            string sql = $"SELECT ID_Tisch, ID_Kellner FROM Tisch WHERE ID_Kellner = {id_Kellner}";
            SQLiteConnection sqliteconnection = new SQLiteConnection(strTemp);
            sqliteconnection.Open();
            SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection);
            SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader();

            while (sqlitereader.Read())
            {
                Tisch tisch = new Tisch();
                tisch.ID_Tisch = sqlitereader.GetInt32(0);
                tisch.ID_Kellner = sqlitereader.GetInt32(1);
                tische.Add(tisch);
            }
            sqliteconnection.Close();
            return tische;
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

        /// <summary>
        /// Duplicate of GetTischeForKellner
        /// <para>Gets a list of all tables, the waiter is assigned to</para>
        /// </summary>
        /// <param name="id_Kellner"></param>
        /// <returns></returns>
        [Obsolete]
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
