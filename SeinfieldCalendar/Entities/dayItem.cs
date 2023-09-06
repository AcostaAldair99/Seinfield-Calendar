
using SeinfieldCalendar.Entities;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                BorderBrush = null,
                BorderThickness = new Thickness(0)
        };



            return this.btnContainer;
        }

        private Canvas setButtonElements()
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


            Image img = getImage();



            Canvas cv = new Canvas();
            cv.Height = 40;
            cv.Width = 40;

            Canvas.SetLeft(lblDay, 10);
            Canvas.SetTop(lblDay, 0);
            cv.Children.Add(lblDay);

            Canvas.SetLeft(img, 0);
            Canvas.SetTop(img, 0);
            cv.Children.Add(img);
           

            Canvas.SetZIndex(img, cv.Children.Count - 1);

            return cv;
        }

        private void setLinkToChain()
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = SeinfieldCalendar.Entities.MessageBoxEx.askQuestionYesNo(this.wm, "¿You dont break the chain?", "Warning",buttons,icon);

           if (result == MessageBoxResult.Yes)
           {
                
                Canvas cv = this.btnContainer.Content as Canvas;
                if (cv != null)
                {
                    //Use the return of query to analize if affected a least one row
                    if (this.itemConnection.insertDate(this.dateOfItem.ToString("dd/MM/yyyy")) > 0)
                    {
                        setChain();
                        MessageBoxEx.Show(" ¡ Congratulations You Don't break the chain !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private Image getImage()
        {
            Image image =   new Image
            {
                Source = new BitmapImage(new Uri("/Resources/link.png", UriKind.Relative)),
                Width = 40,
                Height = 40,
                Opacity = 0
            };
            return image;
        }

        public void setChain()
        {
            Canvas cv = this.btnContainer.Content as Canvas;
            int desiredIndex = 1;

            if (desiredIndex >= 0 && desiredIndex < cv.Children.Count)
            {
                UIElement childAtIndex = cv.Children[desiredIndex];

                childAtIndex.Opacity = 0.4;
            }
            //this.btnContainer.BorderBrush = Brushes.Green;
            //this.btnContainer.BorderThickness = new Thickness(0, 0, 0, 0);

        }

        public void setCurrentDay()
        {
            SolidColorBrush borderColor = new SolidColorBrush(Colors.Red);
            this.btnContainer.BorderBrush = borderColor;
            this.btnContainer.BorderThickness = new Thickness(1, 1, 1, 1);
            this.btnContainer.Click += (sender,e) => setLinkToChain();
        }

    }
}
