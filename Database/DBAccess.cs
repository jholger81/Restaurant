using Restaurant.Models;
using System.Collections.Generic;
using System;
using System.Data.SQLite;
using System.Linq;

namespace Restaurant.Database
{
    public static class DBAccess
    {
        public static string dbConnection = "Data Source=Database.db3";

        #region invoice
        public static List<Bestellposition> ReconstructPositions(List<Bestellposition> positions)
        {
            List<Bestellposition> recoveredPositions = new List<Bestellposition>();
            foreach (var pos in positions)
            {
                Bestellposition recoveredPos = GetOrderPosition(pos.ID_Bestellposition);
                recoveredPositions.Add(recoveredPos);
            }
            return recoveredPositions;
        }

        public static void PayBillPartially(List<Bestellposition> positions, int trinkgeld)
        {
            string sqlUpdateOrderPosition, sqlInsertInvoice, sqlInsertInvoicePos, sqlInsertIntoBestellung_Rechnung;
            int idInvoice;

            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                // Rechnung anlegen
                sqlInsertInvoice = @$"
                    INSERT INTO Rechnung(Trinkgeld)
                    VALUES({trinkgeld})
                    RETURNING ID_Rechnung";
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertInvoice, sqliteconnection))
                {
                    var idObj = sqlitecommand.ExecuteScalar();
                    idInvoice = Convert.ToInt32(idObj);
                }
            }

            List<int> idList = new List<int>();
            foreach (var pos in positions)
            {
                using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
                {
                    sqliteconnection.Open();
                    // Bestellpositionen auf bezahlt setzen
                    // geliefert = 2 ==> bezahlt
                    sqlUpdateOrderPosition = @$"
                        UPDATE Bestellposition
                        SET Geliefert = 2
                        WHERE ID_BESTELLPOSITION = {pos.ID_Bestellposition};";
                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlUpdateOrderPosition, sqliteconnection))
                    {
                        sqlitecommand.ExecuteNonQuery();
                        pos.Geliefert = Bestellposition.PositionsStatus.Bezahlt;
                    }

                    // Position in Rechnungspositionen einfügen und zur Rechnung verknüpfen
                    sqlInsertInvoicePos = $@"
                        INSERT INTO Rechnungposition(ID_Rechnung, ID_Artikel)
                        VALUES({idInvoice}, {pos.ID_Artikel})";
                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertInvoicePos, sqliteconnection))
                    {
                        sqlitecommand.ExecuteNonQuery();
                    }

                    if (!idList.Contains(pos.ID_Bestellung))
                        idList.Add(pos.ID_Bestellung);
                }
            }

            foreach (int id in idList)
            {
                using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
                {
                    sqliteconnection.Open();
                    sqlInsertIntoBestellung_Rechnung = @$"
                        INSERT INTO Bestellung_Rechnung(ID_Bestellung, ID_Rechnung)
                        VALUES({id}, {idInvoice})";
                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertIntoBestellung_Rechnung, sqliteconnection))
                    {
                        sqlitecommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<Rechnungposition> GetPaidPositions(int tischnummer)
        {
            List<int> idList = new List<int>();
            List<Rechnungposition> rechnungPositionen= new List<Rechnungposition>();
            // select all ID_Bestellung for a given ID_Tisch, where there are open order positions
            string sqlGetPositions = @$"
                    SELECT 
                        o.ID_Bestellung
                    FROM Bestellung o
                    JOIN Bestellposition op
                        ON o.ID_Bestellung = op.ID_Bestellung
                    WHERE
                        op.Geliefert <> 2
                    AND 
                        o.ID_Tisch = {tischnummer}
                    GROUP BY o.ID_Bestellung;";
            
            SQLiteCommand sqlitecommand;
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (sqlitecommand = new SQLiteCommand(sqlGetPositions, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqliteReader = sqlitecommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            int idBestellung = sqliteReader.GetInt32(0);
                            idList.Add(idBestellung);
                        }
                    }
                }
                foreach (int id in idList)
                {
                    // select all invoice positions for a given order (ID_Bestellung)
                    string sqlGetInvoicePositionsToOrders = @$"
                            SELECT
                                ip.ID_Rechnungposition,
                                ip.ID_Rechnung,
                                ip.ID_Artikel
                            FROM Rechnung i
                            JOIN Bestellung_Rechnung oi
                                ON i.ID_Rechnung = oi.ID_Rechnung
                            JOIN Bestellung o
                                ON oi.ID_Bestellung = o.ID_Bestellung
                            JOIN Rechnungposition ip
	                            ON i.ID_Rechnung = ip.ID_Rechnung
                            WHERE 
                                o.ID_Bestellung = {id}
                            
                                ";
                    using (sqlitecommand = new SQLiteCommand(sqlGetInvoicePositionsToOrders, sqliteconnection))
                    {
                        using (SQLiteDataReader reader = sqlitecommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Rechnungposition rechnungPos = new Rechnungposition();
                                rechnungPos.ID_Rechnungposition = reader.GetInt32(0);
                                rechnungPos.ID_Rechnung = reader.GetInt32(1);
                                rechnungPos.ID_Artikel = reader.GetInt32(2);
                                rechnungPositionen.Add(rechnungPos);
                            }
                        }
                    }
                }
                foreach (var pos in rechnungPositionen)
                {
                    // get article for given article id (ID_Artikel)
                    string sqlGetArticle = @$"
                            SELECT
                                ID_Artikel, 
                                Name, 
                                Preis, 
                                Kategorie
                            FROM Artikel
                            WHERE 
                                ID_Artikel = {pos.ID_Artikel}";
                    using (sqlitecommand = new SQLiteCommand(sqlGetArticle, sqliteconnection))
                    {
                        using (SQLiteDataReader reader = sqlitecommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Artikel artikel = new Artikel();
                                artikel.ID_Artikel = reader.GetInt32(0);
                                artikel.Name = reader.GetString(1);
                                artikel.Preis = reader.GetInt32(2);
                                artikel.Kategorie = reader.GetString(3);
                                pos.Artikel = artikel;
                            }
                        }
                    }
                }
            }
            return rechnungPositionen;
        }

        #endregion

        #region order
        public enum GetOrderMode
        {
            All = 0,
            Open = 1,
            Closed = 2
        }

        public static List<Bestellposition> GetAllOpenOrderPositions()
        {
            List<Bestellposition> positions = new List<Bestellposition>();

            string sqlcommand = @$"
                SELECT 
                    ID_Bestellposition, 
                    ID_Artikel, 
                    ID_Bestellung, 
                    Geliefert, 
                    Extras
                FROM Bestellposition 
                WHERE Geliefert = 0";

            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        if (!sqlitereader.HasRows)
                            return null;

                        // read orders
                        while (sqlitereader.Read())
                        {
                            Bestellposition pos = new Bestellposition();
                            pos.ID_Bestellposition = sqlitereader.GetInt32(0);
                            pos.ID_Artikel = sqlitereader.GetInt32(1);
                            pos.ID_Bestellung = sqlitereader.GetInt32(2);
                            pos.Geliefert = (Bestellposition.PositionsStatus)(sqlitereader.GetInt32(3));
                            if (sqlitereader["Extras"] == DBNull.Value)
                                pos.Extras = null;
                            else
                                pos.Extras = sqlitereader.GetString(4);
                            positions.Add(pos);
                        }
                    }
                }
            }
            return positions;
        }

        public static List<Bestellung> GetOrder(int id_Tisch, GetOrderMode mode = GetOrderMode.All)
        {
            List<Bestellung> bestellungen = new List<Bestellung>();
            List<Bestellposition> removeList = new List<Bestellposition>();
            List<Bestellung> removeOrderList = new List<Bestellung>();

            string sqlcommand = @$"
                SELECT 
                    ID_Bestellung, 
                    Datum, 
                    ID_Tisch 
                FROM Bestellung 
                WHERE ID_Tisch = {id_Tisch}";

            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        if (!sqlitereader.HasRows)
                            return null;

                        // read orders
                        while (sqlitereader.Read())
                        {
                            Bestellung bestellung = new Bestellung();
                            bestellung.ID_Bestellung = sqlitereader.GetInt32(0);
                            bestellung.Datum = DateTime.Parse(sqlitereader.GetString(1));
                            bestellung.ID_Tisch = sqlitereader.GetInt32(2);
                            bestellungen.Add(bestellung);
                        }
                    }
                }
            }

            foreach (var bestellung in bestellungen)
            {
                // read order positions
                bestellung.Positionen = GetOrderPositions(bestellung.ID_Bestellung);

                // remove open or closed positions, if necessary
                if (mode == GetOrderMode.Open)
                {
                    foreach (var pos in bestellung.Positionen)
                    {
                        if ((int)pos.Geliefert == (int)Bestellposition.PositionsStatus.Bezahlt)
                        {
                            removeList.Add(pos);
                        }
                    }
                }
                if (mode == GetOrderMode.Closed)
                {
                    foreach (var pos in bestellung.Positionen)
                    {
                        if ((int)pos.Geliefert != (int)Bestellposition.PositionsStatus.Bezahlt)
                        {
                            removeList.Add(pos);
                        }
                    }
                }
                foreach (var pos in removeList)
                {
                    bestellung.Positionen.Remove(pos);
                }

                // read articles for order positions
                foreach (Bestellposition position in bestellung.Positionen)
                {
                    position.Artikel = GetArticle(position.ID_Artikel);
                }
            }

            foreach(var bestellung in bestellungen)
            {
                if (bestellung.Positionen?.Count == 0 || bestellung.Positionen == null)
                {
                    removeOrderList.Add(bestellung);
                }
            }
            foreach(var bestellung in removeOrderList)
            {
                bestellungen.Remove(bestellung);
            }
            return bestellungen;
        }

        public static Bestellposition GetOrderPosition(int id_Bestellungposition)
        {
            Bestellposition position = new Bestellposition();
            
            string sqlcommand = @$"
                SELECT 
                    ID_Bestellposition, 
                    ID_Artikel, 
                    ID_Bestellung, 
                    Extras, 
                    Geliefert 
                FROM Bestellposition 
                WHERE ID_Bestellposition = {id_Bestellungposition}";

            using (SQLiteConnection sqliteconnection =  new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        if (sqlitereader.Read())
                        {
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
                            position.Geliefert = (Bestellposition.PositionsStatus)sqlitereader.GetInt32(4);
                        }
                    }
                }
            }
            return position;
        }

        public static List<Bestellposition> GetOrderPositions(int id_Bestellung)
        {
            List<Bestellposition> positionen = new List<Bestellposition>();
            string sqlcommand = @$"
                SELECT 
                    ID_Bestellposition, 
                    ID_Artikel, 
                    ID_Bestellung, 
                    Extras, 
                    Geliefert 
                FROM Bestellposition 
                WHERE ID_Bestellung = {id_Bestellung}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
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
                            position.Geliefert = (Bestellposition.PositionsStatus)sqlitereader.GetInt32(4);
                            positionen.Add(position);
                        }
                    }
                }
            }
            return positionen;
        }

        public static void InsertOrder(Bestellung neueBestellung)
        {
            string sqlInsertOrderPos;
            int idBestellung;
            int idBestellPos;
            string bestellzeitpunkt = neueBestellung.Datum.ToString("yyyy-MM-dd hh:mm:ss");
            string sqlInsertOrder = @$"
                INSERT INTO Bestellung(Datum, ID_Tisch) 
                VALUES('{bestellzeitpunkt}',{neueBestellung.ID_Tisch}) 
                RETURNING ID_Bestellung";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertOrder, sqliteconnection))
                {
                    var idBestellungObj = sqlitecommand.ExecuteScalar();
                    idBestellung = Convert.ToInt32(idBestellungObj);
                    neueBestellung.ID_Bestellung = (int)idBestellung;
                }

                foreach (Bestellposition position in neueBestellung.Positionen)
                {
                    sqlInsertOrderPos = @$"
                        INSERT INTO Bestellposition(ID_Artikel, ID_Bestellung, Extras, Geliefert) 
                        VALUES({position.ID_Artikel}, {idBestellung}, '{position.Extras}', {(int)position.Geliefert}) 
                        RETURNING ID_Bestellposition";

                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertOrderPos, sqliteconnection))
                    {
                        var bestellPosIDObj = sqlitecommand.ExecuteScalar();
                        idBestellPos = Convert.ToInt32(bestellPosIDObj);
                        position.ID_Bestellposition = idBestellPos;
                    }
                }
            }
        }

        public static void UpdateOrder(Bestellung neueBestellung)
        {
            string sqlUpdateOrderPos;
            int idBestellPos;
            string bestellzeitpunkt = neueBestellung.Datum.ToString("yyyy-MM-dd hh:mm:ss");
            string sqlInsertOrder = @$"
                UPDATE Bestellung 
                SET 
                    Datum = '{bestellzeitpunkt}', 
                    ID_Tisch = {neueBestellung.ID_Tisch}) 
                WHERE ID_Bestellung = {neueBestellung.ID_Bestellung}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlInsertOrder, sqliteconnection))
                {
                    sqlitecommand.ExecuteNonQuery();
                }
                foreach (Bestellposition position in neueBestellung.Positionen)
                {
                    sqlUpdateOrderPos = @$"
                        UPDATE Bestellposition 
                        SET 
                            ID_Artikel = {position.ID_Artikel}, 
                            Extras = {position.Extras}, 
                            Geliefert = {position.Geliefert} 
                        WHERE ID_Position = {position.ID_Bestellposition}";
                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlUpdateOrderPos, sqliteconnection))
                    {
                        var bestellPosIDObj = sqlitecommand.ExecuteScalar();
                        idBestellPos = Convert.ToInt32(bestellPosIDObj);
                        position.ID_Bestellposition = idBestellPos;
                    }
                }
            }
                
        }
        #endregion

        #region table
        public static List<Tisch> GetTablesWithOpenOrders()
        {
            var tischliste = new List<Tisch>();
            tischliste = GetAlleTische();
            var removeList = new List<Tisch>();
            foreach (var tisch in tischliste)
            {
                if (GetOrder(tisch.ID_Tisch) == null)
                {
                    removeList.Add(tisch);
                }
            }
            foreach (var tisch in removeList)
            {
                tischliste.Remove(tisch);
            }
            return tischliste;
        }

        public static Tisch GetTisch(int id_Tisch)
        {
            Tisch tisch = new Tisch();
            string sqlcommand = @$"
                SELECT 
                    ID_Tisch, 
                    ID_Kellner 
                FROM Tisch 
                WHERE ID_Tisch = {id_Tisch}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            tisch.ID_Tisch = sqlitereader.GetInt32(0);
                            tisch.ID_Kellner = sqlitereader.GetInt32(1);
                        }
                    }
                }
            }
            return tisch;
        }

        public static List<Tisch> GetAlleTische()
        {
            List<Tisch> tische = new List<Tisch>();
            string sqlcommand = @$"
                SELECT ID_Tisch 
                FROM Tisch";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            var tisch = new Tisch();
                            tisch = GetTisch(sqlitereader.GetInt32(0));
                            tische.Add(tisch);
                        }
                    }
                }
            }
            return tische;
        }

        public static List<Tisch> GetTischeForKellner(int id_Kellner)
        {
            List<Tisch> tische = new List<Tisch>();
            string sqlcommand = @$"
                SELECT 
                    ID_Tisch, 
                    ID_Kellner 
                FROM Tisch 
                WHERE ID_Kellner = {id_Kellner}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlcommand, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            Tisch tisch = new Tisch();
                            tisch.ID_Tisch = sqlitereader.GetInt32(0);
                            tisch.ID_Kellner = sqlitereader.GetInt32(1);
                            tische.Add(tisch);
                        }
                    }
                }
            }
            return tische;
        }

        public static void SwitchTables(int fromTable, int toTable)
        {
            string sqlUpdateTable = @$"
                UPDATE Bestellung 
                SET ID_Tisch = {toTable} 
                WHERE ID_Tisch = {fromTable} 
                AND ID_Bestellung IN (
                    SELECT 
                        ID_Bestellung 
                    FROM Bestellposition 
                    WHERE geliefert IN (0, 1)
                )";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlUpdateTable, sqliteconnection))
                {
                    sqliteconnection.Open();
                    sqlitecommand.ExecuteNonQuery();
                }
            }
        }

        public static void SwitchWaiterForTable(int id_Kellner, int id_Tisch)
        {
            string sqlUpdateTable = @$"
                UPDATE Tisch 
                SET ID_Kellner = {id_Kellner} 
                WHERE ID_Tisch = {id_Tisch}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlUpdateTable, sqliteconnection))
                {
                    sqliteconnection.Open();
                    sqlitecommand.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region article
        public static Artikel GetArticle(int id_Artikel)
        {
            Artikel artikel = new Artikel();
            string sql = @$"
                SELECT 
                    ID_Artikel, 
                    Name, 
                    Preis 
                FROM Artikel 
                WHERE ID_Artikel = {id_Artikel}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        sqlitereader.Read();
                        artikel.ID_Artikel = sqlitereader.GetInt32(0);
                        artikel.Name = sqlitereader.GetString(1);
                        artikel.Preis = sqlitereader.GetInt32(2);
                    }
                }
            }
            return artikel;
        }

        public static List<Artikel> GetAlleGetraenke()
        {
            List<Artikel> getraenke = new List<Artikel>();
            string sql = @$"
                SELECT 
                    ID_Artikel, 
                    Name, 
                    Preis, 
                    Kategorie 
                FROM Artikel 
                WHERE Kategorie = 'Getraenk'";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            Artikel getraenk = new Artikel();
                            getraenk.ID_Artikel = sqlitereader.GetInt32(0);
                            getraenk.Name = sqlitereader.GetString(1);
                            getraenk.Preis = sqlitereader.GetInt32(2);
                            getraenk.Kategorie = sqlitereader.GetString(3);
                            getraenke.Add(getraenk);
                        }
                    }
                }
            }
            return getraenke;
        }

        public static List<Artikel> GetAlleSpeisen()
        {
            List<Artikel> speisen = new List<Artikel>();
            string sql = @$"
                SELECT 
                    ID_Artikel, 
                    Name, 
                    Preis, 
                    Kategorie 
                FROM Artikel 
                WHERE Kategorie = 'Speise'";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            Artikel speise = new Artikel();
                            speise.ID_Artikel = sqlitereader.GetInt32(0);
                            speise.Name = sqlitereader.GetString(1);
                            speise.Preis = sqlitereader.GetInt32(2);
                            speise.Kategorie = sqlitereader.GetString(3);
                            speisen.Add(speise);
                        }
                    }
                }
            }
            return speisen;
        }
        
        public static List<Artikel> GetAlleDesserts()
        {
            List<Artikel> desserts = new List<Artikel>();
            string sql = @$"
                SELECT 
                    ID_Artikel, 
                    Name, 
                    Preis, 
                    Kategorie 
                FROM Artikel 
                WHERE Kategorie = 'Dessert'";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            Artikel dessert = new Artikel();
                            dessert.ID_Artikel = sqlitereader.GetInt32(0);
                            dessert.Name = sqlitereader.GetString(1);
                            dessert.Preis = sqlitereader.GetInt32(2);
                            dessert.Kategorie = sqlitereader.GetString(3);
                            desserts.Add(dessert);
                        }
                    }
                }                    
            }
            return desserts;
        }
        #endregion

        #region waiter
        public static Kellner GetKellner(int id_Kellner)
        {
            Kellner kellner = new Kellner();
            string sql = @$"
                SELECT 
                    ID_Kellner, 
                    Nachname, 
                    Vorname 
                FROM Kellner 
                WHERE ID_Kellner = {id_Kellner}";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            kellner.ID_Kellner = sqlitereader.GetInt32(0);
                            kellner.Nachname = sqlitereader.GetString(1);
                            kellner.Vorname = sqlitereader.GetString(2);
                        }
                    }
                }    
            }
            return kellner;
        }

        public static Kellner GetKellner(string nachname, string vorname = null)
        {
            Kellner kellner = new Kellner();
            string sql = @$"
                SELECT 
                    ID_Kellner, 
                    Nachname, 
                    Vorname 
                FROM Kellner 
                WHERE Nachname = '{nachname}'";
            if (vorname != null)
                sql += $" AND Vorname = '{vorname}'";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            kellner.ID_Kellner = sqlitereader.GetInt32(0);
                            kellner.Nachname = sqlitereader.GetString(1);
                            kellner.Vorname = sqlitereader.GetString(2);
                        }
                    }
                }
            }
            return kellner;
        }

        public static int CheckLogin(Kellner kellner)
        {
            string sql = @$"
                SELECT 
                    ID_Kellner
                FROM Kellner 
                WHERE LoginName = '{kellner.LoginName}'
                AND Passwort = '{kellner.Passwort}'";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        if (sqlitereader.Read())
                        {
                            return sqlitereader.GetInt32(0);
                        }
                    }
                }
            }
            return 0;
        }
        #endregion

        #region statistics
        public static int GetDailyIncome(DateTime day)
        {
            List<int> invoiceIDs = new List<int>();
            List<int> prices = new List<int>();
            int sum = 0;

            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                string sqlGetInvoiceIDs = $@"
                SELECT
	                I.ID_Rechnung
                FROM Bestellung o
                JOIN Bestellung_Rechnung O2I
	                ON O.ID_Bestellung = O2I.ID_Bestellung
                JOIN Rechnung I
	                ON O2I.ID_Rechnung = I.ID_Rechnung
                WHERE
	                O.Datum like '{day.ToString("yyyy-MM-dd")}%'
                GROUP BY I.ID_Rechnung";
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlGetInvoiceIDs, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            invoiceIDs.Add(sqlitereader.GetInt32(0));
                        }
                    }
                }
            }



            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                sqliteconnection.Open();
                foreach (int invoiceID in invoiceIDs)
                {
                    string sqlGetSumOfArticlePricesForInvoice = $@"
                    SELECT
	                    SUM(A.Preis)
                    FROM Rechnung I
                    JOIN Rechnungposition IP
	                    ON I.ID_Rechnung = IP.ID_Rechnung
                    JOIN Artikel A
	                    ON IP.ID_Artikel = A.ID_Artikel
                    WHERE
	                    I.ID_Rechnung = {invoiceID};";
                    using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqlGetSumOfArticlePricesForInvoice, sqliteconnection))
                    {
                        using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                        {
                            while (sqlitereader.Read())
                            {
                                prices.Add(sqlitereader.GetInt32(0));
                            }
                            sum = prices.Sum();
                        }
                    }
                }
            }
            return sum;
        }

        public static int GetDailyTips(DateTime day)
        {
            List<int> tips = new List<int>();
            int sum;

            string sql = $@"
                SELECT
	                I.Trinkgeld,
	                I.ID_Rechnung
                FROM Bestellung o
                JOIN Bestellung_Rechnung O2I
	                ON O.ID_Bestellung = O2I.ID_Bestellung
                JOIN Rechnung I
	                ON O2I.ID_Rechnung = I.ID_Rechnung
                WHERE
	                O.Datum like '{day.ToString("yyyy-MM-dd")}%'
                GROUP BY I.ID_Rechnung";
            using (SQLiteConnection sqliteconnection = new SQLiteConnection(dbConnection))
            {
                using (SQLiteCommand sqlitecommand = new SQLiteCommand(sql, sqliteconnection))
                {
                    sqliteconnection.Open();
                    using (SQLiteDataReader sqlitereader = sqlitecommand.ExecuteReader())
                    {
                        while (sqlitereader.Read())
                        {
                            tips.Add(sqlitereader.GetInt32(0));
                        }
                        sum = tips.Sum();
                    }
                }
            }
            return sum;
        }
        #endregion
    }
}
