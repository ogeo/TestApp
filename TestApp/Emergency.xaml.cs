using System;
using System.Device.Location;
using System.Linq;
using System.Windows;

namespace TestApp
{
    public partial class Emergency
    {
        GeoCoordinateWatcher _watcher;

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
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) { MovementThreshold = 0 };
            _watcher.PositionChanged += WatcherPositionChanged;
            _watcher.StatusChanged += _watcher_StatusChanged;
            _watcher.Start();
        }

        void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled: break;
                case GeoPositionStatus.NoData: break;
            }
        }

        void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var epl = e.Position.Location;
            textBlock1.Text = epl.Latitude.ToString("0.00000") + " " + epl.Longitude.ToString("0.00000") + " (" + epl.HorizontalAccuracy + ")";
        }
    }
}