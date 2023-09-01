﻿using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SeinfieldCalendar.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        private readonly SQLiteConnection sqlConnection;
        private Dictionary<string, string> savedDates = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            sqlConnection = createConnectionToSqlite();
            createCalendar();
        }


        private void getData()
        {
            sqlConnection.Open();
            string selectQuery = "SELECT * FROM chain_dates";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, sqlConnection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string name = reader.GetString(1);
                        savedDates.Add(id, name);
                        // Process other columns as needed
                    }
                }
            }
            sqlConnection.Close();
        }

        private SQLiteConnection createConnectionToSqlite()
        {
            string relativeFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "data.db");
            string connection = $"Data Source={relativeFolderPath};Version=3;New=False;Compress=True;";
            SQLiteConnection sqlite_conn = new SQLiteConnection(connection);
            return sqlite_conn;
        }

        private void setStyles()
        {

            SolidColorBrush buttonBackgroundDays = new SolidColorBrush(Colors.MediumSlateBlue);
            buttonBackgroundDays.Freeze();

            Style buttonStyleDays = new Style(typeof(Button));


            buttonStyleDays.Setters.Add(new Setter(Button.BackgroundProperty, buttonBackgroundDays));
            buttonStyleDays.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));

            Resources.Add("ButtonBackgroundBrush", buttonBackgroundDays);
            Resources.Add("ButtonStyleDays", buttonStyleDays);





        }

        private void createCalendar()
        {
            getData();
            setStyles();
            setCalendarColumns();
            setCalendarRows();

            currentDate = DateTime.Today;
            
            setLabelDays();
            setMonthButtons();

            monthLabel.Content = new DateTime(currentDate.Year, currentDate.Month+1, 1).ToString("yyyy MMM");
            
            setDaysInCalendar(currentDate);

           

        }

        private void setMonthButtons()
        {
            nextMonthButton.Style = (Style)Resources["ButtonStyleDays"];
            prevMonthButton.Style = (Style)Resources["ButtonStyleDays"];

            nextMonthButton.Cursor = Cursors.Hand;
            prevMonthButton.Cursor = Cursors.Hand;

            nextMonthButton.Content = ">";
            prevMonthButton.Content = "<";

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

                calendarItem date = new calendarItem(monthCalendar,day);
                if (savedDates.ContainsValue(date.getDayOfItemAsString()))
                {
                    date.setChainItem();
                }


                Grid.SetRow(date.btnContainer, row);
                Grid.SetColumn(date.btnContainer, column);
                seinfieldCalendar.Children.Add(date.btnContainer);
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


            daysRow.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week1.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week2.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week3.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week4.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);
            week5.Height = new GridLength(ROW_HEIGHT, GridUnitType.Pixel);

            seinfieldCalendar.RowDefinitions.Add(daysRow);
            seinfieldCalendar.RowDefinitions.Add(week1);
            seinfieldCalendar.RowDefinitions.Add(week2);
            seinfieldCalendar.RowDefinitions.Add(week3);
            seinfieldCalendar.RowDefinitions.Add(week4);
            seinfieldCalendar.RowDefinitions.Add(week5);

        }

        private Label getLabelItem(string content)
        {
            Label n = new Label();
            n.Content = content;
            n.HorizontalAlignment = HorizontalAlignment.Center;
            n.VerticalAlignment = VerticalAlignment.Center;
            return n;
        }

    }

}
