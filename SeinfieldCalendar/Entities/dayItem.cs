using Newtonsoft.Json.Linq;
using SeinfieldCalendar.Entities;
using SeinfieldCalendar.Properties;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SeinfieldCalendar.Model
{
    public class dayItem
    {
        private readonly string pathToDb = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }
        private MainWindow wm;

        private readonly SqliteConnector itemConnection;

        public dayItem(MainWindow wm,DateTime date,int day)
        {
            dateOfItem = new DateTime(date.Year,date.Month,day);
            btnContainer = setButton();
            itemConnection = new SqliteConnector(pathToDb);
            this.wm = wm;
        }

        public DateTime getDayOfItem()
        {
            return this.dateOfItem;
        }

        public Button setButton()
        {
            this.btnContainer = new Button
            {
                Content = setButtonElements(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Cursor = Cursors.Hand,
                Background = Brushes.Transparent,
                
            };



            return this.btnContainer;
        }

        private StackPanel setButtonElements()
        {
            Label lblDay = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = this.dateOfItem.Day,
                Foreground = Brushes.White,
                //FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI")
            };
            StackPanel cv = new StackPanel();
            cv.Children.Add(lblDay);
            return cv;
        }

        private void setLinkToChain()
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = SeinfieldCalendar.Entities.MessageBoxEx.askQuestionYesNo(this.wm, "¿You dont break the chain?", "Warning",buttons,icon);

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
            Image image =   new Image
            {
                Source = new BitmapImage(new Uri("/Resources/link.png", UriKind.Relative)),
                Width = 30,
                Height = 30,
                Opacity = 0.5,
            };
            return image;
        }

        public void setChain()
        {
            this.btnContainer.BorderBrush = null;
            this.btnContainer.BorderThickness = new Thickness(0);
            StackPanel sp = this.btnContainer.Content as StackPanel;
            //sp.Children.RemoveAt(0);
            Image img = getImage();
            sp.Children.Add(img);
            this.btnContainer.IsEnabled = false;
        }

        public void setCurrentDay()
        {
            SolidColorBrush borderColor = new SolidColorBrush(Colors.Red);
            this.btnContainer.BorderBrush = borderColor;
            this.btnContainer.BorderThickness = new Thickness(2, 2, 2, 2);
            this.btnContainer.Click += (sender,e) => setLinkToChain();
            this.btnContainer.IsEnabled = true;
        }

    }
}
