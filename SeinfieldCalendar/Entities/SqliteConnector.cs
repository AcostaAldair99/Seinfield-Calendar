using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace SeinfieldCalendar.Entities
{
    public class SqliteConnector
    {
        private string pathToDb { get; set; }
        private readonly SQLiteConnection conn;
        public SqliteConnector(string pathToDb)
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

        public List<string> getDates(DateTime currentDate)
        {
            string month = currentDate.ToString("MM");
            string year = currentDate.ToString("yyyy");
            //In this list we store all the dates
            List<string> dates = new List<string>();
            this.conn.Open();
            string selectQuery = $"SELECT * FROM chain_dates WHERE month = '{month}' AND year='{year}'";
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, this.conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string date = reader.GetString(1)+"/"+reader.GetString(2)+"/"+reader.GetString(3);
                        dates.Add(date);
                    }
                }
            }
            this.conn.Close();
            return dates;
        }

        public int insertDate(string date)
        {
            this.conn.Open();
            int rowsAffected = 0;
            string[] dateValues = date.Split('/');
            string insertQuery = "INSERT INTO chain_dates (id,day,month,year) VALUES (@Value1,@Value2,@Value3,@Value4)";
            string id = ComputeSHA256Hash(date);
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, this.conn))
            {

                command.Parameters.AddWithValue("@Value1", id);
                command.Parameters.AddWithValue("@Value2", dateValues[0]);
                command.Parameters.AddWithValue("@Value3", dateValues[1]);
                command.Parameters.AddWithValue("@Value4", dateValues[2]);

                //The functions ExecuteNonQuery returns the number of affected rows
                //so we gonna use the integer to check if the query is successfull
                rowsAffected = command.ExecuteNonQuery();
            }
            this.conn.Close();
            return rowsAffected;
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
