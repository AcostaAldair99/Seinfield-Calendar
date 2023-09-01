using Microsoft.EntityFrameworkCore.Query.Internal;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SeinfieldCalendar.Model
{
    public class calendarItem
    {
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }
        private  SQLiteConnection connection { get; set; }

        private readonly DateTime currentDate = DateTime.Today;

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

            //This shit is for styles of the buttons
            //n.Style = (Style)Resources["ButtonStyleDays"];
            SolidColorBrush borderColor = new SolidColorBrush(Colors.Red);
          
            btnContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            btnContainer.VerticalAlignment = VerticalAlignment.Stretch;

            DateTime tt = new DateTime(currentDate.Year, currentDate.Month + 1, 1);
            if(this.dateOfItem == tt)
            {
                btnContainer.Cursor = Cursors.Hand;
                btnContainer.Click += (sender, e) => setLinkToChain(this.btnContainer);
                btnContainer.BorderBrush = borderColor;
                btnContainer.BorderThickness = new Thickness(4, 4, 4, 4); // Left, Top, Right, Bottom
            }

           

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

                    changeStateOfButton(sp);
                    this.btnContainer.BorderBrush = null;
                    this.btnContainer.BorderThickness = new Thickness(0); // Left, Top, Right, Bottom
                    insertData(this.dateOfItem.ToString("dd/MM/yyyy"));
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

        //The only reason to use SHA256 is because i read a article about it and i wanna use it
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
                changeStateOfButton(sp);
            }
        }

        public void changeStateOfButton(StackPanel sp)
        {
            sp.Children.RemoveAt(0);
            Image img = getImage();
            sp.Children.Add(img);
        }

    }
}
