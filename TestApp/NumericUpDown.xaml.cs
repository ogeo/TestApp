using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TestApp
{
    public partial class NumericUpDown : UserControl
    {
        private bool _remove;

        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        public int Value
        {
            get { return int.Parse(textBlock2.Text); }
            set { textBlock2.Text = value.ToString(); }
        }

        public string LabelString
        {
            get { return textBlock1.Text; }
            set { textBlock1.Text = value; }
        }

        public NumericUpDown()
        {
            InitializeComponent();
        }

        private bool CheckLimit(int val)
        {
            return !(val > MaxValue || val < MinValue);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLimit(int.Parse(textBlock2.Text) - 1)) textBlock2.Text = (int.Parse(textBlock2.Text) - 1).ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLimit(int.Parse(textBlock2.Text) + 1)) textBlock2.Text = (int.Parse(textBlock2.Text) + 1).ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock2.Text = ((MaxValue + MinValue) / 2).ToString();
        }
    }
}
