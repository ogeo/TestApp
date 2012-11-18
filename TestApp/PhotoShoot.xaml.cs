using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;

namespace TestApp
{
    public partial class PhotoShoot : PhoneApplicationPage
    {
        private int _savedCounter;
        private PhotoCamera _cam;
        private readonly MediaLibrary _library = new MediaLibrary();

        public PhotoShoot()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Camera.IsCameraTypeSupported(CameraType.Primary))
            {
                _cam = new PhotoCamera(CameraType.Primary);

                _cam.CaptureCompleted += CamCaptureCompleted;
                _cam.CaptureImageAvailable += CamCaptureImageAvailable;
                _cam.CaptureThumbnailAvailable += CamCaptureThumbnailAvailable;

                viewfinderBrush.SetSource(_cam);
            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (_cam == null) return;
            _cam.Dispose();

            _cam.CaptureCompleted -= CamCaptureCompleted;
            _cam.CaptureImageAvailable -= CamCaptureImageAvailable;
            _cam.CaptureThumbnailAvailable -= CamCaptureThumbnailAvailable;
        }

        private void CamCaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            _savedCounter++;
        }

        private void CamCaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            string fileName = _savedCounter + ".jpg";

            try
            {
                _library.SavePictureToCameraRoll(fileName, e.ImageStream);
                e.ImageStream.Seek(0, SeekOrigin.Begin);

                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (
                        IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create,
                                                                                  FileAccess.Write))
                    {

                        byte[] readBuffer = new byte[4096];
                        int bytesRead;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {

                e.ImageStream.Close();
            }

        }

        public void CamCaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            string fileName = _savedCounter + "_th.jpg";

            try
            {
                using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (
                        IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create,
                                                                                  FileAccess.Write))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytesRead;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {
                e.ImageStream.Close();
            }
        }

        private void ShutterButtonClick1(object sender, RoutedEventArgs e)
        {
            if (_cam == null) return;
            _cam.CaptureImage();
        }
    }
}