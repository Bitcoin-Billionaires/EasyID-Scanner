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
using System.Windows.Threading;

namespace Scanner_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            lbl_time.Content = DateTime.Now.ToString("h:mm tt");
            lbl_date.Content = DateTime.Now.ToString("d");
        }

        private void btn_scan_Click(object sender, RoutedEventArgs e)
        {
            tb_dob.Dispatcher.Invoke(new Action(() => tb_dob.Text = "18/10/1999"));
            tb_age.Dispatcher.Invoke(new Action(() => tb_age.Text = "21"));
            tb_name.Dispatcher.Invoke(new Action(() => tb_name.Text = "Jamey Tubbz"));
            tb_expiry.Dispatcher.Invoke(new Action(() => tb_expiry.Text = "01/22"));
            tb_gender.Dispatcher.Invoke(new Action(() => tb_gender.Text = "Sigma Male"));

            Image id = new Image();
            id.Source = new BitmapImage(new Uri(@"img/id-photo.jpg", UriKind.Relative));
            lbl_id.Dispatcher.Invoke(new Action(() => lbl_id.Visibility = Visibility.Hidden));
            border_id.Dispatcher.Invoke(new Action(() => border_id.Child = id));

            Image patron = new Image();
            patron.Source = new BitmapImage(new Uri(@"img/patron-photo.jpeg", UriKind.Relative));
            lbl_patron.Dispatcher.Invoke(new Action(() => lbl_patron.Visibility = Visibility.Hidden));
            border_patron.Dispatcher.Invoke(new Action(() => border_patron.Child = patron));

            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "Scan Successful!"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "ID scanned as \"Jamey Tubbz\". Please pass a verdict on ID validity."));
        }

        private void btn_approve_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User has been Approved!", "Approved", MessageBoxButton.OK, MessageBoxImage.Information);
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Approved"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"Jamey Tubbz\" has been Approved!"));
        }

        private void btn_reject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User has been Rejected!", "Rejected", MessageBoxButton.OK, MessageBoxImage.Error);
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Rejected"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"Jamey Tubbz\" has been Rejected!"));
        }

        private void btn_flag_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User flagged in system for further review!", "Flagged", MessageBoxButton.OK, MessageBoxImage.Warning);
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Flagged"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"Jamey Tubbz\" has been Flagged and added to the database!"));
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btn_manual_Click(object sender, RoutedEventArgs e)
        {
            tb_dob.Dispatcher.Invoke(new Action(() => tb_dob.Text = "        /        /"));
            tb_age.Dispatcher.Invoke(new Action(() => tb_age.Text = ""));
            tb_name.Dispatcher.Invoke(new Action(() => tb_name.Text = ""));
            tb_expiry.Dispatcher.Invoke(new Action(() => tb_expiry.Text = "                                   /"));
            tb_gender.Dispatcher.Invoke(new Action(() => tb_gender.Text = ""));

            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "Manual Entry Mode"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "ID being manually entered. Please pass a verdict on ID validity."));
        }
    }
}
