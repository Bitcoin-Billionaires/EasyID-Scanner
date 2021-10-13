using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using CsvHelper;
using Image = System.Windows.Controls.Image;
using Window = OpenCvSharp.Window;

namespace Scanner_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private DispatcherTimer WebcamTimer;
        private VideoCapture capture;
        private Mat frame;
        private Bitmap image;
        private Image currentCamFrame;


        public MainWindow()
        {
            InitializeComponent();

            // Time and Date DispatcherTimer
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();

            // Webcam DispatcherTimer
            WebcamTimer = new DispatcherTimer();
            WebcamTimer.Interval = TimeSpan.FromMilliseconds(1);
            WebcamTimer.Tick += WebcamTick;
        }

        private void WebcamToggle(object sender, RoutedEventArgs e)
        {
            if (!WebcamTimer.IsEnabled)
            {
                WebcamTimer.Start(); 
                currentCamFrame = new Image();
                currentCamFrame.HorizontalAlignment = HorizontalAlignment.Center;
                currentCamFrame.VerticalAlignment = VerticalAlignment.Center;
                currentCamFrame.Stretch = Stretch.UniformToFill;
                capture = new VideoCapture(0);
                capture.Open(0);
                lbl_patron.Dispatcher.Invoke(new Action(() => lbl_patron.Visibility = Visibility.Hidden));
                border_patron.Dispatcher.Invoke(new Action(() => border_patron.Child = currentCamFrame));
            }
            else
            {
                WebcamTimer.Stop();
                capture.Release();
                lbl_patron.Dispatcher.Invoke(new Action(() => lbl_patron.Visibility = Visibility.Visible));
                border_patron.Dispatcher.Invoke(new Action(() => border_patron.Child.Visibility = Visibility.Hidden));
            }
        }

        private void WebcamTick(object sender, EventArgs e)
        {
            frame = new Mat();

            if (capture.IsOpened())
            {
                capture.Read(frame);
                image = BitmapConverter.ToBitmap(frame);

                currentCamFrame.Dispatcher.Invoke(new Action((() => currentCamFrame.Source = BitmapToImageSource(image))));
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
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

            var id = new Image();
            id.Source = new BitmapImage(new Uri(@"img/id-photo.jpg", UriKind.Relative));
            //id.Source = new BitmapImage(new Uri(@"https://thispersondoesnotexist.com/image", UriKind.RelativeOrAbsolute));
            lbl_id.Dispatcher.Invoke(new Action(() => lbl_id.Visibility = Visibility.Hidden));
            border_id.Dispatcher.Invoke(new Action(() => border_id.Child = id));

            var patron = new Image();
            patron.Source = new BitmapImage(new Uri(@"img/patron-photo.jpeg", UriKind.Relative));
            lbl_patron.Dispatcher.Invoke(new Action(() => lbl_patron.Visibility = Visibility.Hidden));
            border_patron.Dispatcher.Invoke(new Action(() => border_patron.Child = patron));

            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "Scan Successful!"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "ID scanned as \"" + tb_name.Text + "\". Please pass a verdict on ID validity."));
        }

        private void btn_reject_Click(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1(System.Windows.Window.GetWindow(this));
            w.Show();
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
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "ID being manually altered."));
        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow w = new HistoryWindow(System.Windows.Window.GetWindow(this));
            w.Show();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PatronRejected()
        {
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Rejected"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"" + tb_name.Text + "\" has been Rejected!"));
        }

        public void PatronFlagRejected(object sender, RoutedEventArgs e)
        {
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Flagged & Rejected"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"" + tb_name.Text + "\" has been flagged for future reference!"));
        }

        public void PatronApproved(object sender, RoutedEventArgs e)
        {
            lbl_status.Dispatcher.Invoke(new Action(() => lbl_status.Content = "ID Approved"));
            lbl_statusinfo.Dispatcher.Invoke(new Action(() => lbl_statusinfo.Content = "\"" + tb_name.Text + "\" has been Approved!"));
        }
    }
}
