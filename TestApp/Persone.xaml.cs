using System;
using System.Device.Location;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace TestApp
{
    public partial class Persone
    {
        private GeoCoordinateWatcher _watcher;
        private GeoCoordinate _currentCoords;

        public Persone()
        {
            InitializeComponent();

            textBox3.InputScope = new InputScope { Names = { new InputScopeName { NameValue = InputScopeNameValue.TelephoneNumber } } };
            textBox2.InputScope = new InputScope { Names = { new InputScopeName { NameValue = InputScopeNameValue.EmailNameOrAddress } } };
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) { MovementThreshold = 0 };
            _watcher.PositionChanged += WatcherPositionChanged;
            _watcher.StatusChanged += _watcher_StatusChanged;
            _watcher.Start();
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                wc.Encoding = Encoding.UTF8;
                wc.UploadStringAsync(
                    new Uri("http://sample-env-kmuwwhu5qf.elasticbeanstalk.com/rest/hosting/people" + BuildUrl()),
                    "POST", "");
                wc.UploadStringCompleted += wc_UploadStringCompleted;
            }
            catch
            {
                MessageBox.Show("La persona è stata inserita con successo.", "Persona inserita", MessageBoxButton.OK);
            }
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            MessageBox.Show("La persona è stata inserita con successo.", "Persona inserita", MessageBoxButton.OK);
        }

        string BuildUrl()
        {
            string s = "?";

            s += "name=" + textBox1.Text;
            s += "&email=" + textBox2.Text;
            s += "&cellNumber=" + textBox3.Text;
            s += "&longitude=" + _currentCoords.Longitude.ToString("0.0000").Replace(',', '.');
            s += "&latitude=" + _currentCoords.Latitude.ToString("0.0000").Replace(',', '.');

            return s;
        }
    }
}