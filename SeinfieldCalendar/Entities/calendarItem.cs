using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SeinfieldCalendar.Model;
using System.IO;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;

namespace SeinfieldCalendar.Entities
{
    public class calendarItem
    {
        private readonly string pathToDb = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        private readonly int COLUMN_WIDTH = 40;
        private readonly int ROW_HEIGHT = 40;
        private DateTime currentDate;
        private readonly List<string> days = new List<string> { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private readonly MainWindow mainWindow;
        private readonly DateTime realDate;
        private SolidColorBrush HOVER_HEX_COLOR;
        private SolidColorBrush FONTS_COLOR;
        private SolidColorBrush WINDOW_COLOR;
        private bool THEME = false;

        public calendarItem(MainWindow mw,DateTime currentDate)
        {
            this.mainWindow = mw;
            this.currentDate = currentDate;
            this.realDate = currentDate;
        }


        public void createCalendar()
        {
            setCalendarColumns();
            setCalendarRows();
            setCalendarElements();
            //15616d
            setElementsWithTheme("#001524", "#15616d", "#FFFFFF");
        }

        public void setElementsWithTheme(string WindowColor,string hoverColor, string labelColor)
        {
            Color windowColor = (Color)ColorConverter.ConvertFromString(WindowColor);
            WINDOW_COLOR = new SolidColorBrush(windowColor);
            this.mainWindow.Background = WINDOW_COLOR;
            
            Color lblcolor = (Color)ColorConverter.ConvertFromString(labelColor);
            FONTS_COLOR = new SolidColorBrush(lblcolor);

            Color hovercolor = (Color)ColorConverter.ConvertFromString(hoverColor);
            HOVER_HEX_COLOR = new SolidColorBrush(hovercolor);


            this.mainWindow.titleLabel.Foreground = FONTS_COLOR;
            this.mainWindow.monthLabel.Foreground = FONTS_COLOR;

            StackPanel lblprev = this.mainWindow.prevMonthButton.Content as StackPanel;
            int desiredIndex = 0;

            if (desiredIndex >= 0 && desiredIndex < lblprev.Children.Count)
            {
                UIElement childAtIndex = lblprev.Children[desiredIndex];
                Label l = childAtIndex as Label;
                l.Foreground = FONTS_COLOR;
            }


            StackPanel lblnext = this.mainWindow.nextMonthButton.Content as StackPanel;
            int index = 0;

            if (index >= 0 && index < lblnext.Children.Count)
            {
                UIElement childAtIndex = lblnext.Children[index];
                Label l = childAtIndex as Label;
                l.Foreground = FONTS_COLOR;
            }

            setLabelDays();
            setDaysOfTheMonth();
        }



        private void setCalendarElements()
        {
            this.mainWindow.titleLabel.Foreground = FONTS_COLOR;
            this.mainWindow.monthLabel.Content = new DateTime(this.currentDate.Year, this.currentDate.Month, 1).ToString("yyyy MMM");
            this.mainWindow.monthLabel.Foreground = FONTS_COLOR;


            this.mainWindow.nextMonthButton.Cursor = Cursors.Hand;
            this.mainWindow.prevMonthButton.Cursor = Cursors.Hand;

            this.mainWindow.nextMonthButton.Content = setContentButtons(">");
            this.mainWindow.prevMonthButton.Content = setContentButtons("<");

            this.mainWindow.nextMonthButton.Background = Brushes.Transparent;
            this.mainWindow.prevMonthButton.Background = Brushes.Transparent;


            this.mainWindow.nextMonthButton.Click += (sender, e) => changeCurrentMonth(1);
            this.mainWindow.prevMonthButton.Click += (senter, e) => changeCurrentMonth(-1);
            this.mainWindow.nextMonthButton.MouseLeave += (sender, e) => changeButtonColor(this.mainWindow.nextMonthButton,Brushes.Transparent);
            this.mainWindow.nextMonthButton.MouseEnter += (sender, e) => changeButtonColor(this.mainWindow.nextMonthButton, HOVER_HEX_COLOR);

            this.mainWindow.prevMonthButton.MouseLeave += (sender, e) => changeButtonColor(this.mainWindow.prevMonthButton, Brushes.Transparent);
            this.mainWindow.prevMonthButton.MouseEnter += (sender, e) => changeButtonColor(this.mainWindow.prevMonthButton,HOVER_HEX_COLOR);


            //The configuration for the theme button

            //Get Image for the button

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("/Resources/lightbulb.png", UriKind.Relative)),
                Width = 20,
                Height = 20,
                Opacity = 0.9,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            StackPanel sp = new StackPanel();

            sp.Children.Add(image);
            this.mainWindow.btnTheme.Background = Brushes.Transparent;
            this.mainWindow.btnTheme.Width = 30;
            this.mainWindow.btnTheme.Height = 30;
            this.mainWindow.btnTheme.HorizontalAlignment = HorizontalAlignment.Center;
            this.mainWindow.btnTheme.VerticalAlignment  = VerticalAlignment.Center;
            this.mainWindow.btnTheme.Content = sp;
            this.mainWindow.btnTheme.Cursor = Cursors.Hand;
            this.mainWindow.btnTheme.MouseLeave += (sender, e) => changeButtonColor(this.mainWindow.btnTheme, Brushes.Transparent);
            this.mainWindow.btnTheme.MouseEnter += (sender, e) => changeButtonColor(this.mainWindow.btnTheme, HOVER_HEX_COLOR);
            this.mainWindow.btnTheme.Click += (sender, e) => setTheme();
        }

        private void setTheme()
        {
            if(THEME == false)
            {
                setElementsWithTheme("#e8eddf", "#cfdbd5", "#000000");
                changeImageButton("/Resources/sleep-mode.png");
                THEME = true;
            }
            else
            {
                setElementsWithTheme("#001524", "#15616d", "#FFFFFF");
                changeImageButton("/Resources/lightbulb.png");
                THEME = false;
            }
        }

        private void changeImageButton(string pathToImage)
        {
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(pathToImage, UriKind.Relative)),
                Width = 20,
                Height = 20,
                Opacity = 0.9,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            StackPanel sp = this.mainWindow.btnTheme.Content as StackPanel;
            sp.Children.RemoveAt(0);
            sp.Children.Add(image);
        }

        private void changeButtonColor(Button btn,SolidColorBrush buttonColor)
        {
            StackPanel sp = btn.Content as StackPanel;
            sp.Background = buttonColor;
        }


        private StackPanel setContentButtons(string value)
        {
            Label content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = value,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI"),
                //Width = 35,
                Background = Brushes.Transparent
            };
            StackPanel cv = new StackPanel();
            cv.Children.Add(content);
            cv.Width = 35;
            return cv;
        }

        private void setLabelDays()
        {
            this.mainWindow.seinfieldCalendar.Children.Clear();
            int i = 0;
            foreach (string day in days)
            {
                Label lbl = new Label
                {
                    Content = day,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = FONTS_COLOR,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Segoe UI")
                };
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, i);
                this.mainWindow.seinfieldCalendar.Children.Add(lbl);
                i++;
            }
        }

        private void changeCurrentMonth(int value)
        {
            DateTime nextMonth = this.currentDate.AddMonths(value);
            this.mainWindow.monthLabel.Content = nextMonth.ToString("yyyy MMM");
            this.currentDate = nextMonth;
            this.mainWindow.seinfieldCalendar.Children.Clear();
            setLabelDays();
            setDaysOfTheMonth();
        }


        private void setCalendarColumns()
        {
            ColumnDefinition Sunday = new ColumnDefinition();
            ColumnDefinition Monday = new ColumnDefinition();
            ColumnDefinition Tuesday = new ColumnDefinition();
            ColumnDefinition Wednesday = new ColumnDefinition();
            ColumnDefinition Thurday = new ColumnDefinition();
            ColumnDefinition Friday = new ColumnDefinition();
            ColumnDefinition Saturday = new ColumnDefinition();

            Sunday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Monday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Tuesday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Wednesday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Thurday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Friday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);
            Saturday.Width = new GridLength(COLUMN_WIDTH, GridUnitType.Pixel);

            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Sunday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Monday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Tuesday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Wednesday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Thurday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Friday);
            this.mainWindow.seinfieldCalendar.ColumnDefinitions.Add(Saturday);

        }
        private void setCalendarRows()
        {
            RowDefinition daysRow = new RowDefinition();
            RowDefinition week1 = new RowDefinition();
            RowDefinition week2 = new RowDefinition();
            RowDefinition week3 = new RowDefinition();
            RowDefinition week4 = new RowDefinition();
            RowDefinition week5 = new RowDefinition();


            daysRow.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week1.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week2.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week3.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week4.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week5.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);

            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(daysRow);
            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(week1);
            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(week2);
            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(week3);
            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(week4);
            this.mainWindow.seinfieldCalendar.RowDefinitions.Add(week5);

        }


        public void setDaysOfTheMonth()
        {
            SqliteConnector itemConnection = new SqliteConnector(pathToDb);
            List<string> savedDates = itemConnection.getDates(this.currentDate);
            int daysOfMonth = DateTime.DaysInMonth(this.currentDate.Year,this.currentDate.Month);
            DateTime firstDayOfMonth = new DateTime(this.currentDate.Year, this.currentDate.Month, 1);


            for (int day = 1; day <= daysOfMonth; day++)
            {
                int row = (day + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;
                int column = ((day - 1) + (int)firstDayOfMonth.DayOfWeek) % 7;

                dayItem date = new dayItem(this.mainWindow, this.currentDate, day,FONTS_COLOR,HOVER_HEX_COLOR);
                string data = date.getDayOfItem().ToString("dd/MM/yyyy");

                if(this.realDate == date.getDayOfItem() && !savedDates.Contains(data))
                {
                    date.setCurrentDay();
                }
                if (savedDates.Contains(data) )
                {
                    date.setChain();
                }

                Grid.SetRow(date.btnContainer, row);
                Grid.SetColumn(date.btnContainer, column);
                mainWindow.seinfieldCalendar.Children.Add(date.btnContainer);
            }

        }

    }
}
