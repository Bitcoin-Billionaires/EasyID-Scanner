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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private MainWindow parent;

        public Window1(Window parent)
        {
            InitializeComponent();
            this.parent = (MainWindow) parent;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RejectPatron(object sender, RoutedEventArgs e)
        {
            parent.PatronRejected();
            this.Close();
        }

        private void FlagRejectPatron(object sender, RoutedEventArgs e)
        {
            parent.PatronFlagRejected(sender, e);
            this.Close();
        }
    }
}
