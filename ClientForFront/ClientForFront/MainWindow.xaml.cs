using System;
using System.Windows;
using OpenCvSharp;


namespace ClientForFront
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string RTSP_URL = this.textBox.Text.Trim();
                VideoCapture capture = new VideoCapture(RTSP_URL);
                //capture.Open();

                //Capture
            }
            catch (Exception ex)
            {

            }
        }
    }
}
