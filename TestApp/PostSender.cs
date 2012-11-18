using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Phone.Controls;

namespace TestApp
{
    public delegate void WorkDoneEventHandler(string result);

    public class PostSender
    {
        readonly string _url;

        public PostSender(string url)
        {
            _url = url;
        }

        public void SendData(Dictionary<string, string> data)
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string d = "test=dsadsa";
            client.Headers[HttpRequestHeader.ContentLength] = d.Length.ToString();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.UploadProgressChanged += new UploadProgressChangedEventHandler(client_UploadProgressChanged);
            client.UploadStringAsync(new Uri(_url), d);
        }

        void client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Debug.WriteLine(string.Format("Progress: {0} ", e.ProgressPercentage)); 
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Result);
        }
    }
}
