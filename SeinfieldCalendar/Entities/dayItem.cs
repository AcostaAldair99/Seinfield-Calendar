
using SeinfieldCalendar.Entities;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SeinfieldCalendar.Model
{
    public class dayItem
    {
        private readonly string pathToDb = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        public DateTime dateOfItem { get; set; }
        public Button btnContainer { get; set; }
        private MainWindow wm;
        private readonly string HOVER_HEX_COLOR = "#15616d";
        private readonly SqliteConnector itemConnection;
        private SolidColorBrush labelColor;
        private SolidColorBrush hoverColor;

        public dayItem(MainWindow wm,DateTime date,int day,SolidColorBrush labelColor,SolidColorBrush hoverColor)
        {
            this.labelColor = labelColor;
            this.hoverColor = hoverColor;
            this.dateOfItem = new DateTime(date.Year,date.Month,day);
            this.btnContainer = setButton();
            this.itemConnection = new SqliteConnector(pathToDb);
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

            this.btnContainer.MouseEnter += (sender,e) => changeButtonColor(Brushes.Transparent,hoverColor,labelColor);
            this.btnContainer.MouseLeave += (sender, e) => changeButtonColor(hoverColor, Brushes.Transparent, labelColor);

            return this.btnContainer;
        }




        private void changeButtonColor(SolidColorBrush fromColor ,SolidColorBrush toColor,SolidColorBrush lblColor)
        {
            Border bd = this.btnContainer.Content as Border;
            animateHoverEvent(bd, fromColor.Color, toColor.Color);
        }

        private void animateHoverEvent(Border objective,Color fromColor,Color toColor)
        {
            ThicknessAnimation dA = new ThicknessAnimation
            {
                From = new Thickness(0.1),
                To = new Thickness(0.9),
                Duration = TimeSpan.FromMilliseconds(20)
            };

            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = fromColor,
                To = toColor,
                Duration = TimeSpan.FromMilliseconds(20)
            };


            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(colorAnimation);
            //storyboard.Children.Add(dA);

            Storyboard.SetTarget(colorAnimation, objective);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));

            //Storyboard.SetTarget(dA, objective);
            //Storyboard.SetTargetProperty(dA, new PropertyPath("Border.BorderThicknessProperty"));

            storyboard.Begin();
        }


    private Border setButtonElements()
        {
            Label lblDay = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = this.dateOfItem.Day,
                Foreground = labelColor,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 14,
            };


            Image img = getImage();



            Canvas cv = new Canvas();
            cv.Height = 40;
            cv.Width = 40;

            Canvas.SetLeft(lblDay, 10);
            Canvas.SetTop(lblDay, 0);
            cv.Children.Add(lblDay);

            Canvas.SetLeft(img, 5);
            Canvas.SetTop(img, 0);
            cv.Children.Add(img);
           

            Canvas.SetZIndex(img, cv.Children.Count - 1);

            Border bd = new Border();
            bd.Background = Brushes.Transparent;
            bd.BorderBrush = labelColor;
            bd.BorderThickness = new Thickness(0.1);
            bd.Child = cv;
            
           
            return bd;
        }

        private void setLinkToChain()
        {
            MessageBoxResult result = SeinfieldCalendar.Entities.MessageBoxEx.askQuestionYesNo(this.wm, "¿You dont break the chain?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

           if (result == MessageBoxResult.Yes)
           {
                    //Use the return of query to analize if affected a least one row
                    if (this.itemConnection.insertDate(this.dateOfItem.ToString("dd/MM/yyyy")) > 0)
                    {
                      
                       MessageBoxEx.Show(" ¡ Congratulations You Don't break the chain !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                       setChain();
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
                Opacity = 0
            };
            return image;
        }

        public void setChain()
        {
            Border bd = this.btnContainer.Content as Border;
            Canvas cv = bd.Child as Canvas;
            int desiredIndex = 1;

            if (desiredIndex >= 0 && desiredIndex < cv.Children.Count)
            {
                UIElement childAtIndex = cv.Children[desiredIndex];
                Image img = childAtIndex as Image;
                animateOpacityChain(img);
            
            }
        }
        private void animateOpacityChain(Image img)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = 0.4,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(da);

            // Set the target of the animation
            Storyboard.SetTarget(da, img);
            Storyboard.SetTargetProperty(da, new PropertyPath(Image.OpacityProperty));

            // Start the animation
            storyboard.Begin();
        }




        public void setCurrentDay()
        {
            this.btnContainer.Click += (sender,e) => setLinkToChain();
        }

    }
}
