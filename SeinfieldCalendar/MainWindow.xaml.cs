using SeinfieldCalendar.Entities;
using System;
using System.Windows;


namespace SeinfieldCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DateTime currentDate = DateTime.Today;
            calendarItem calendar = new calendarItem(this,currentDate);
            calendar.createCalendar();
        }


    }

}
