using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeinfieldCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int COLUMN_WIDTH = 40;
        int ROW_HEIGHT = 40;

        public MainWindow()
        {
            InitializeComponent();
            /// setCalendar();
            
            createCalendar();
        }

        private void createCalendar()
        {
            setCalendarColumns();
            setCalendarRows();
            DateTime currentDate = DateTime.Today;
            int daysOfMonth = DateTime.DaysInMonth(currentDate.Year,currentDate.Month);
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            monthLabel.Content = new DateTime(currentDate.Year, currentDate.Month, 1).ToString("MMMM");

            for(int day = 1; day <= daysOfMonth; day++)
            {
                int row = (day + (int)firstDayOfMonth.DayOfWeek - 1) / 7;
                int column = ((day - 1) + (int)firstDayOfMonth.DayOfWeek) % 7;

                Button date = getButtonItem(day);
                Console.WriteLine(date.ToString());

                
                Grid.SetRow(date, row);
                Grid.SetColumn(date, column);
                seinfieldCalendar.Children.Add(date);
            }


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

            seinfieldCalendar.ColumnDefinitions.Add(Sunday);
            seinfieldCalendar.ColumnDefinitions.Add(Monday);
            seinfieldCalendar.ColumnDefinitions.Add(Tuesday);
            seinfieldCalendar.ColumnDefinitions.Add(Wednesday);
            seinfieldCalendar.ColumnDefinitions.Add(Thurday);
            seinfieldCalendar.ColumnDefinitions.Add(Friday);
            seinfieldCalendar.ColumnDefinitions.Add(Saturday);
            
        }
        private void setCalendarRows()
        {
            RowDefinition week1 = new RowDefinition();
            RowDefinition week2 = new RowDefinition();
            RowDefinition week3 = new RowDefinition();
            RowDefinition week4 = new RowDefinition();
            RowDefinition week5 = new RowDefinition();
            RowDefinition week6 = new RowDefinition();

            week1.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week2.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week3.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week4.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week5.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week6.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);

            seinfieldCalendar.RowDefinitions.Add(week1);
            seinfieldCalendar.RowDefinitions.Add(week2);
            seinfieldCalendar.RowDefinitions.Add(week3);
            seinfieldCalendar.RowDefinitions.Add(week4);
            seinfieldCalendar.RowDefinitions.Add(week5);
            seinfieldCalendar.RowDefinitions.Add(week6);
        }

        private void setCalendar()
        {
            setCalendarColumns();
            setCalendarRows();

            DateTime currentDate = DateTime.Now;
            DateTime date = new DateTime(currentDate.Year, currentDate.Month, 1);
            DayOfWeek dw = date.DayOfWeek;

            //Here we gonna create an label and put it in the grid 
            for(int i = 0 ; i< seinfieldCalendar.ColumnDefinitions.Count ; i++)
            {
                for(int j = 0;j<seinfieldCalendar.RowDefinitions.Count;j++)
                {
                    Button day = getButtonItem((int)(dw));
                    seinfieldCalendar.Children.Add(day);
                    Grid.SetColumn(day, i);
                    Grid.SetRow(day, j);
                 
                }
            }


        }

        //This will be every day on the calendar
        private Button getButtonItem(int value)
        {
            Button n = new Button();
            n.Content = Convert.ToString(value);
            n.HorizontalAlignment = HorizontalAlignment.Stretch;
            n.VerticalAlignment = VerticalAlignment.Stretch;
            n.Click += (sender, e) => dateClick(value);
            return n;
        }

        private void dateClick(int day)
        {
            MessageBox.Show($"You click in the date {day}");
        }
    }

}
