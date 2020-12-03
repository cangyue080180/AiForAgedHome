using AIForAgedClient.Helper;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AIForAgedClient.ViewModel
{
    public class HuoChaiAndOriginVideoVM : BaseFourVideoVM
    {
        public UInt32 RoomId { get; set; }

        private TcpClient tcpClient;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSource;
        private readonly int remote_port = 8008;
        private string remote_ip = "127.0.0.1";
        private VideoPlayHelper videoPlayHelper1;

        private async Task<byte[]> Tcp_recv(NetworkStream stream, int data_len)
        {
            byte[] tempBuffer = new byte[data_len];
            try
            {
                int recv_len = await stream.ReadAsync(tempBuffer, 0, tempBuffer.Length);

                while (recv_len < data_len)
                {
                    int temp_recv_len = await stream.ReadAsync(tempBuffer, recv_len, data_len - recv_len);
                    recv_len += temp_recv_len;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("网络断开！");
                //TODO: 提示用户网络存在问题，无法继续通信。
            }
            return tempBuffer;
        }

        public override void Start()
        {
            //清空上次的显示内容
            Image1 = null;
            Image2 = null;
            Image3 = null;
            Image4 = null;

            if (!string.IsNullOrEmpty(Url2))
            {
                videoPlayHelper1 = new VideoPlayHelper(Url2, (x) => { Image2 = x; });
            }

            remote_ip = ConfigurationManager.AppSettings["TcpServerIp"];
            tcpClient = new TcpClient(remote_ip, remote_port);
            Console.WriteLine($"tcp connect: ({remote_ip}, {remote_port})");

            stream = tcpClient.GetStream();

            //发送角色数据包
            Role role = new Role(3, 1, 1);
            byte[] send_bytes = StructToBytesHelper.StructToBytes<Role>(role);
            stream.WriteAsync(send_bytes, 0, send_bytes.Length);
            Console.WriteLine("send_role_packet");

            //发送获取图像请求命令包
            VideoCmd videoCmd = new VideoCmd(1, 5, RoomId, 1);
            byte[] videoCmd_bytes = StructToBytesHelper.StructToBytes<VideoCmd>(videoCmd);
            stream.WriteAsync(videoCmd_bytes, 0, videoCmd_bytes.Length);
            Console.WriteLine("send_get_video_packet");

            //接收图像数据并显示
            cancellationTokenSource = new CancellationTokenSource();
            Task recvVideoTask = new Task(async () =>
            {
                try
                {
                    await RecvVideo(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {

                }
                stream?.Close();
                tcpClient?.Close();
            }, cancellationTokenSource.Token);

            recvVideoTask.Start();
            videoPlayHelper1?.Start();
        }

        private async Task RecvVideo(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                byte[] video_header_bytes = await Tcp_recv(stream, 9);
                VideoHeader videoHeader = StructToBytesHelper.BytesToStruct<VideoHeader>(video_header_bytes);
                if (videoHeader.type == 2)
                {
                    byte[] video_image_bytes = await Tcp_recv(stream, (int)(videoHeader.len) - 4);
                    Console.WriteLine("recv: " + video_image_bytes.Length);
                    BitmapImage bitmap = null;
                    try
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(video_image_bytes);
                        bitmap.EndInit();
                        bitmap.Freeze();

                        if (Url1 != null && videoHeader.cameraId == uint.Parse(Url1))
                        {
                            Image1 = bitmap;
                        }
                        else if (Url2 != null && videoHeader.cameraId == uint.Parse(Url2))
                        {
                            Image2 = bitmap;
                        }
                        else if (Url3 != null && videoHeader.cameraId == uint.Parse(Url3))
                        {
                            Image3 = bitmap;
                        }
                        else if (Url4 != null && videoHeader.cameraId == uint.Parse(Url4))
                        {
                            Image4 = bitmap;
                        }
                        else
                        {

                        }
                    }
                    catch (System.NotSupportedException)
                    {
                        Console.WriteLine("bitmap not support");
                    }
                }
            }

        }

        public override void Stop()
        {
            videoPlayHelper1?.Stop();
            //发送关闭图像命令
            VideoCmd videoCmd = new VideoCmd(1, 5, RoomId, 0);
            byte[] videoCmd_bytes = StructToBytesHelper.StructToBytes<VideoCmd>(videoCmd);
            stream.Write(videoCmd_bytes, 0, videoCmd_bytes.Length);

            cancellationTokenSource.Cancel();

        }


    }
}
