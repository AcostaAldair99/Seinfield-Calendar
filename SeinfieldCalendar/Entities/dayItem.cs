using Newtonsoft.Json.Linq;
using SeinfieldCalendar.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SeinfieldCalendar.Model
{
    public class dayItem
    {
        private readonly string pathToDb = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }

        private sqlite itemConnection;

        public dayItem(DateTime date,int day)
        {
            dateOfItem = new DateTime(date.Year,date.Month,day);
            btnContainer = setButton();
            itemConnection = new sqlite(pathToDb);
        }

        public DateTime getDayOfItem()
        {
            return this.dateOfItem;
        }

        public Button setButton()
        {
            this.btnContainer = new Button();
            this.btnContainer.Content = setButtonElements();
            //n.Style = (Style)Resources["ButtonStyleDays"];

            this.btnContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.btnContainer.VerticalAlignment = VerticalAlignment.Stretch;
            this.btnContainer.Cursor = Cursors.Hand;

            this.btnContainer.Click += (sender, e) => setLinkToChain();

            return btnContainer;
        }

        private StackPanel setButtonElements()
        {
            Label lblDay = new Label();
            lblDay.HorizontalAlignment = HorizontalAlignment.Center;
            lblDay.Content = this.dateOfItem.Day;
            StackPanel cv = new StackPanel();
            cv.Children.Add(lblDay);
            return cv;
        }

        private void setLinkToChain()
        {
            MessageBoxResult result = MessageBox.Show("¿You dont break the chain?", "Question",
                                                      MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                StackPanel sp = this.btnContainer.Content as StackPanel;
                if (sp != null)
                {
                    //Use the return of query to analize if affected a least one row
                    if (this.itemConnection.insertDate(this.dateOfItem.ToString("dd/MM/yyyy")) > 0)
                    {
                        setChain();
                    }
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

        public void setChain()
        {
            StackPanel sp = this.btnContainer.Content as StackPanel;
            sp.Children.RemoveAt(0);
            Image img = getImage();
            sp.Children.Add(img);
        }

    }
}
