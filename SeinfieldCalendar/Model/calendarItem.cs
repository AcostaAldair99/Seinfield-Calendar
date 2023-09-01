using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SeinfieldCalendar.Model
{
    public class calendarItem
    {
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }
        private  SQLiteConnection connection { get; set; }


        public calendarItem(DateTime date,int day,SQLiteConnection conn)
        {
            this.dateOfItem = new DateTime(date.Year,date.Month,day);
            this.btnContainer = setButton();
            this.connection = conn;
        }

        public string getDayOfItemAsString()
        {
            return this.dateOfItem.ToString("dd/MM/yyyy");
        }

        public Button setButton()
        {
            btnContainer = new Button();
            btnContainer.Content = setButtonElements(this.dateOfItem.Day);
            //n.Style = (Style)Resources["ButtonStyleDays"];

            btnContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            btnContainer.VerticalAlignment = VerticalAlignment.Stretch;
            btnContainer.Cursor = Cursors.Hand;

            btnContainer.Click += (sender, e) => setLinkToChain(this.btnContainer);

            return btnContainer;
        }

        private StackPanel setButtonElements(int value)
        {
            Label lblDay = new Label();
            lblDay.HorizontalAlignment = HorizontalAlignment.Center;
            lblDay.Content = Convert.ToString(value);

            StackPanel cv = new StackPanel();

            cv.Children.Add(lblDay);
            return cv;
        }

        private void setLinkToChain(Button btn)
        {
            MessageBoxResult result = MessageBox.Show("¿You dont break the chain?", "Question",
                                                      MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                StackPanel sp = btn.Content as StackPanel;
                if (sp != null)
                {
                    sp.Children.RemoveAt(0);
                    Image img = getImage();
                    sp.Children.Add(img);
                    insertData(this.dateOfItem.ToString("dd/MM/yyyy"));
                    btn.IsEnabled = false;
                }
            }
        }
        private Image getImage()
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("/Resources/xxx.jpg", UriKind.Relative));
            image.Width = 40;
            image.Height = 40;
            return image;
        }

        private void insertData(string date)
        {
            connection.Open();

            string insertQuery = "INSERT INTO chain_dates (id,date) VALUES (@Value1,@Value2)";
            string id = ComputeSHA256Hash(date);

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {

                command.Parameters.AddWithValue("@Value1", id);
                command.Parameters.AddWithValue("@Value2", date);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }


        private string ComputeSHA256Hash(string input)
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

        public void setChainItem()
        {
            StackPanel sp = this.btnContainer.Content as StackPanel;
            if (sp != null)
            {
                sp.Children.RemoveAt(0);
                Image img = getImage();
                sp.Children.Add(img);
                btnContainer.IsEnabled = false;
            }
        }

    }
}
