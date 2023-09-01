using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    public class calendarItem
    {
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }

        public calendarItem(DateTime date,int day)
        {
            dateOfItem = new DateTime(date.Year,date.Month,day);
            btnContainer = setButton();
        }

        public DateTime getDayOfItem()
        {
            return this.dateOfItem;
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
                    Label lbl = sp.Children[0] as Label;
                    //String day = lbl.Content.ToString();
                    sp.Children.RemoveAt(0);
                    Image img = getImage();
                    sp.Children.Add(img);
                    //DateTime dateEstablished = new DateTime(currentDate.Year, currentDate.Month, int.Parse(day));
                    //string date = dateEstablished.ToString("dd/MM/yyyy");
                    //insertData(date);
                    //btn.IsEnabled = false;
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

    }
}
