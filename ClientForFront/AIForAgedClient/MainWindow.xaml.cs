using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace AIForAgedClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly VideoCapture capture;
        private readonly BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();

            capture = new VideoCapture();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //capture.Open(0);//open local camera.
            capture.Open("http://ivi.bupt.edu.cn/hls/cctv5phd.m3u8");
            if (!capture.IsOpened())
            {
                Close();
                return;
            }

            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker)sender;

            while (!bgWorker.CancellationPending)
            {
                if (capture.IsDisposed)
                    break;
                using (var frameMat = capture.RetrieveMat())
                {
                    //var rects = cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));
                    //if (rects.Length > 0)
                    //{
                    //    Cv2.Rectangle(frameMat, rects[0], Scalar.Red);
                    //}

                    var frameBitmap = MatToBitmapImage(frameMat);
                    bgWorker.ReportProgress(0, frameBitmap);
                }
                Thread.Sleep(100);
            }
        }

        public Bitmap MatToBitmap(Mat image)
        {
            return BitmapConverter.ToBitmap(image);
        }
        public BitmapImage MatToBitmapImage(Mat image)
        {
            Bitmap bitmap = MatToBitmap(image);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var imageSource = (BitmapImage)e.UserState;
            this.Image.Source = imageSource;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            capture.Dispose();
            backgroundWorker.CancelAsync();
        }
    }
}
