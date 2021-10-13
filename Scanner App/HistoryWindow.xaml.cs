using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Scanner_App
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private MainWindow parent;

        public HistoryWindow(Window parent)
        {
            InitializeComponent();
            this.parent = (MainWindow) parent;

            List<Patron> patrons = new List<Patron>();
            patrons.Add(new Patron() { Name = "Norwood Stracke", Birthday = new DateTime(1990, 10, 21), Age = 30, Gender = "Male", IdExpiry = new DateTime(2022, 5, 1)});

            dg_history.ItemsSource = patrons;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class Patron
        {
            public string Name { get; set; }
            public DateTime Birthday { get; set; }
            public int Age { get; set; }
            public DateTime IdExpiry { get; set; }
            public string Gender { get; set; }
        }
    }
}
