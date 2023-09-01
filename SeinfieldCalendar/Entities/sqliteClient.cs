using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SeinfieldCalendar.Entities
{
    public class sqliteClient
    {
        private string pathToDb { get; set; }
        private SQLiteConnection conn;
        public sqliteClient(string pathToDb)
        {
            this.pathToDb = pathToDb;
            this.conn = getConnectionToSqlite();
        }


        public SQLiteConnection getConnectionToSqlite()
        {
            string connection = $"Data Source={this.pathToDb};Version=3;New=False;Compress=True;";
            SQLiteConnection sqlite_conn = new SQLiteConnection(connection);
            return sqlite_conn;
        }

        public void getDates()
        {
            this.conn.Open();
            string selectQuery = "SELECT * FROM chain_dates";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, this.conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string name = reader.GetString(1);
                        //savedDates.Add(id, name);
                        // Process other columns as needed
                    }
                }
            }
            this.conn.Close();
        }

        public void insertDates()
        {
            this.conn.Open();

            string insertQuery = "INSERT INTO chain_dates (id,day,month,year) VALUES (@Value1,@Value2,@Value3,@Value4)";
            string id = ComputeSHA256Hash(date);

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, this.conn))
            {

                command.Parameters.AddWithValue("@Value1", id);
                command.Parameters.AddWithValue("@Value2", date);

                command.ExecuteNonQuery();
            }
            this.conn.Close();
        }

        //The only reason to use SHA256 is because i read a article about it and i wanna use it
        public string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

    }
}
