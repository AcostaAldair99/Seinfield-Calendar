using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using SeinfieldCalendar.Properties;
using System.Windows.Media;
using SeinfieldCalendar.Model;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace SeinfieldCalendar.Entities
{
    public class calendarItem
    {
        private readonly string pathToDb = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        private readonly int COLUMN_WIDTH = 40;
        private readonly int ROW_HEIGHT = 40;
        private DateTime currentDate;
        private readonly List<string> days = new List<string> { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private MainWindow mainWindow; 

        public calendarItem(MainWindow mw,DateTime currentDate)
        {
            this.mainWindow = mw;
            this.currentDate = currentDate;
        }


        public void createCalendar()
        {
            setStyles();
            setCalendarColumns();
            setCalendarRows();

            //currentDate = DateTime.Today;

            setLabelDays();
            setMonthButtons();


            mainWindow.monthLabel.Content = new DateTime(this.currentDate.Year, this.currentDate.Month + 1, 1).ToString("yyyy MMM");


            setDaysOfThe();
        }


        private void setMonthButtons()
        {
            //this.mainWindow.nextMonthButton.Style = (Style)Resources["ButtonStyleDays"];
            //this.mainWindow.prevMonthButton.Style = (Style)Resources["ButtonStyleDays"];

            this.mainWindow.nextMonthButton.Cursor = Cursors.Hand;
            this.mainWindow.prevMonthButton.Cursor = Cursors.Hand;

            this.mainWindow.nextMonthButton.Content = ">";
            this.mainWindow.prevMonthButton.Content = "<";

            //this.mainWindow.nextMonthButton.Click += (sender, e) => changeCurrentMonth(1);
            //this.mainWindow.prevMonthButton.Click += (senter, e) => changeCurrentMonth(-1);
        }

        private void setLabelDays()
        {
            int i = 0;
            foreach (string d in days)
            {
                Label lbl = getLabelItem(d);
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, i);
                this.mainWindow.seinfieldCalendar.Children.Add(lbl);
                i++;
            }
        }

        private Label getLabelItem(string content)
        {
            Label n = new Label();
            n.Content = content;
            n.HorizontalAlignment = HorizontalAlignment.Center;
            n.VerticalAlignment = VerticalAlignment.Center;
            return n;
        }


        private void setStyles()
        {

            SolidColorBrush buttonBackgroundDays = new SolidColorBrush(Colors.MediumSlateBlue);
            buttonBackgroundDays.Freeze();

            Style buttonStyleDays = new Style(typeof(Button));


            buttonStyleDays.Setters.Add(new Setter(Button.BackgroundProperty, buttonBackgroundDays));
            buttonStyleDays.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));

            //Resources.Add("ButtonBackgroundBrush", buttonBackgroundDays);
            //Resources.Add("ButtonStyleDays", buttonStyleDays);
            //Resources.Add("ButtonStyleDays", buttonStyleDays);
        }



        /*private void changeCurrentMonth(int value)
        {
            DateTime nextMonth = currentDate.AddMonths(value);
            monthLabel.Content = nextMonth.ToString("yyyy MMMM");
            currentDate = nextMonth;
            seinfieldCalendar.Children.Clear();
            setLabelDays();
            setDaysInCalendar(currentDate);

        }*/


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


        public void setDaysOfThe()
        {
            sqlite itemConnection = new sqlite(pathToDb);
            List<string> savedDates = itemConnection.getDates();
            
            int daysOfMonth = DateTime.DaysInMonth(this.currentDate.Year,this.currentDate.Month);
            DateTime firstDayOfMonth = new DateTime(this.currentDate.Year, this.currentDate.Month, 1);


            for (int day = 1; day <= daysOfMonth; day++)
            {
                int row = (day + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;
                int column = ((day - 1) + (int)firstDayOfMonth.DayOfWeek) % 7;

                dayItem date = new dayItem(this.currentDate, day);
                string data = date.getDayOfItem().ToString("dd/MM/yyyy");
                if (savedDates.Contains(data))
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
