using System;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace TestApp
{
    public partial class Emergency
    {
        private GeoCoordinateWatcher _watcher;
        private GeoCoordinate _currentCoords;

        public Emergency()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PhotoShoot.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var firstOrDefault = NavigationService.BackStack.FirstOrDefault();
            if (firstOrDefault != null && firstOrDefault.Source.ToString() != "/PhotoShoot.xaml") return;
            if (GlobalVars.PhotoName != "")
            {
                button1.Content = "Foto già scattata";
            }
        }

        private void PhoneApplicationPageLoaded(object sender, RoutedEventArgs e)
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) {MovementThreshold = 0};
            _watcher.PositionChanged += WatcherPositionChanged;
            _watcher.StatusChanged += _watcher_StatusChanged;
            _watcher.Start();
        }

        private void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    break;
                case GeoPositionStatus.NoData:
                    break;
            }
        }

        private void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            _currentCoords = e.Position.Location;

            var epl = e.Position.Location;
            textBlock1.Text = epl.Latitude.ToString("0.00000") + ", " + epl.Longitude.ToString("0.00000");
        }

        private void Button3Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            wc.Encoding = Encoding.UTF8;
            wc.UploadStringAsync(new Uri("http://sample-env-kmuwwhu5qf.elasticbeanstalk.com/rest/reviews/" + BuildUrl()), "POST", "");
            wc.UploadStringCompleted += wc_UploadStringCompleted;

              

            WebClient wim = new WebClient();
            wim.Headers["Content-Type"] = "multipart/form-data";
            wim.Encoding = Encoding.UTF8;
            wim.UploadStringAsync(new Uri("http://sample-env-kmuwwhu5qf.elasticbeanstalk.com/rest/reviews/" + BuildUrl()), "POST", "");
            wim.UploadStringCompleted += wc_UploadStringCompleted;
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Result);
        }

        private string BuildUrl()
        {
            string s = "";

            if (radioButton2.IsChecked.HasValue && radioButton2.IsChecked.Value) s += "building";
            else if (radioButton3.IsChecked.HasValue && radioButton3.IsChecked.Value) s += "street";
            else if (radioButton1.IsChecked.HasValue && radioButton1.IsChecked.Value) s += "network";

            s += "?status=" + numericUpDown1.Value;
            s += "&longitude=" + _currentCoords.Longitude.ToString("0.0000").Replace(',', '.');
            s += "&latitude=" + _currentCoords.Latitude.ToString("0.0000").Replace(',', '.');

            return s;
        }
    }
}