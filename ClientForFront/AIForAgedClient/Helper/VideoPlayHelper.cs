using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AIForAgedClient.Helper
{
    public class VideoPlayHelper
    {
        private readonly VideoCapture capture;
        private readonly Task task;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly dynamic videoUrl;
        private readonly Action<BitmapSource> actionWidthVideo;

        /// <summary>
        /// 显示URL视频
        /// </summary>
        /// <param name="url">视频地址</param>
        /// <param name="action">更新图像显示方法</param>
        public VideoPlayHelper(string url, Action<BitmapSource> action)
        {
            videoUrl = url;
            actionWidthVideo = action;

            capture = new VideoCapture();
            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(() =>
            {
                PlayVideo(cancellationTokenSource.Token);
            }, cancellationTokenSource.Token);
            _ = task.ContinueWith((task) =>
              {
                  capture.Dispose();
              });
        }

        /// <summary>
        /// 显示本机摄像头视频
        /// </summary>
        /// <param name="index">摄像头序号</param>
        /// <param name="action">更新图像显示方法</param>
        public VideoPlayHelper(int index, Action<BitmapSource> action)
        {
            videoUrl = index;
            actionWidthVideo = action;

            capture = new VideoCapture();
            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(() => { PlayVideo(cancellationTokenSource.Token); }, cancellationTokenSource.Token);
            _ = task.ContinueWith((task) =>
              {
                  capture.Dispose();
              });
        }

        public void Start()
        {
            //清除上次遗留的图像
            actionWidthVideo(null);
            task.Start();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private void PlayVideo(CancellationToken token)
        {
            capture.Open(videoUrl);
            if (!capture.IsOpened())
            {
                Console.WriteLine($"Camera from {videoUrl} is fail!");
                return;
            }

            while (capture.IsOpened())
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    using (Mat frameMat = capture.RetrieveMat())
                    {
                        //var rects = cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));
                        //if (rects.Length > 0)
                        //{
                        //    Cv2.Rectangle(frameMat, rects[0], Scalar.Red);
                        //}

                        // var frameBitmap = MatToBitmapImage(frameMat);
                        try
                        {
                            WriteableBitmap writeableBitmap = frameMat.ToWriteableBitmap();
                            writeableBitmap.Freeze();
                            actionWidthVideo(writeableBitmap);
                        }
                        catch (System.ArgumentException ex)
                        {
                        }
                    }
                }
                catch (AccessViolationException ex)
                {
                }
                Thread.Sleep(50);
            }
        }

        private Bitmap MatToBitmap(Mat image)
        {
            return BitmapConverter.ToBitmap(image);
        }

        private BitmapImage MatToBitmapImage(Mat image)
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
    }
}