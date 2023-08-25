using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        private readonly int COLUMN_WIDTH = 40;
        private readonly int ROW_HEIGHT = 40;
        private DateTime currentDate;
        private readonly List<string> days = new List<string> { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private readonly Func<int, int, int> add = (a, b) => a + b;
        private readonly Func<int, int, int> subtract = (a, b) => a - b;

        public MainWindow()
        {
            InitializeComponent();
            /// setCalendar();
            createCalendar();
        }

        private void setStyles()
        {

            SolidColorBrush buttonBackgroundBrush = new SolidColorBrush(Colors.LightBlue);
            buttonBackgroundBrush.Freeze();

            Style buttonStyle = new Style(typeof(Button));
            buttonStyle.Setters.Add(new Setter(Button.BackgroundProperty, buttonBackgroundBrush));
            buttonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));

            Resources.Add("ButtonBackgroundBrush", buttonBackgroundBrush);
            Resources.Add("ButtonStyle", buttonStyle);
        }

        private void createCalendar()
        {
            setStyles();
            currentDate = DateTime.Today;
            setCalendarColumns();
            setCalendarRows();
            setLabelDays();

            monthLabel.Content = new DateTime(currentDate.Year, currentDate.Month, 1).ToString("yyyy MMM");
            
            setDaysInCalendar(currentDate);


            nextMonthButton.Click += (sender, e) => changeCurrentMonth(1);
            prevMonthButton.Click += (senter, e) => changeCurrentMonth(-1);

        }

        private void setLabelDays()
        {
            int i = 0;
            foreach (string d in days)
            {
                Label lbl = getLabelItem(d);
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, i);
                seinfieldCalendar.Children.Add(lbl);
                i++;
            }
        }

        private void changeCurrentMonth(int value)
        {
            DateTime nextMonth = currentDate.AddMonths(value);
            monthLabel.Content = nextMonth.ToString("yyyy MMMM");
            currentDate = nextMonth;
            seinfieldCalendar.Children.Clear();
            setLabelDays();
            setDaysInCalendar(currentDate);
            
        }


        private void setDaysInCalendar(DateTime monthCalendar)
        {
            int daysOfMonth = DateTime.DaysInMonth(monthCalendar.Year, monthCalendar.Month);
            DateTime firstDayOfMonth = new DateTime(monthCalendar.Year, monthCalendar.Month, 1);

            for (int day = 1; day <= daysOfMonth; day++)
            {
                int row = (day + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;
                int column = ((day - 1) + (int)firstDayOfMonth.DayOfWeek) % 7;

                Button date = getButtonItem(day);

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
            RowDefinition daysRow = new RowDefinition();
            RowDefinition week1 = new RowDefinition();
            RowDefinition week2 = new RowDefinition();
            RowDefinition week3 = new RowDefinition();
            RowDefinition week4 = new RowDefinition();
            RowDefinition week5 = new RowDefinition();
           /// RowDefinition week6 = new RowDefinition();

            daysRow.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week1.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week2.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week3.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week4.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week5.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            ///week6.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);

            seinfieldCalendar.RowDefinitions.Add(daysRow);
            seinfieldCalendar.RowDefinitions.Add(week1);
            seinfieldCalendar.RowDefinitions.Add(week2);
            seinfieldCalendar.RowDefinitions.Add(week3);
            seinfieldCalendar.RowDefinitions.Add(week4);
            seinfieldCalendar.RowDefinitions.Add(week5);
            //seinfieldCalendar.RowDefinitions.Add(week6);
        }

        //This will be every day on the calendar
        private Button getButtonItem(int value)
        {
            Button n = new Button();
            n.Content = Convert.ToString(value);
            n.Style = (Style)Resources["ButtonStyle"];
            n.HorizontalAlignment = HorizontalAlignment.Stretch;
            n.VerticalAlignment = VerticalAlignment.Stretch;
            n.Click += (sender, e) => dateClick(value);
            return n;
        }

        private Label getLabelItem(string content)
        {
            Label n = new Label();
            n.Content = content;
            n.HorizontalAlignment = HorizontalAlignment.Center;
            n.VerticalAlignment = VerticalAlignment.Center;
            return n;
        }


        private void dateClick(int day)
        {
            MessageBox.Show($"You click in the date {day}");
        }

        private void nextMonthButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
