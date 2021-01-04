using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AIForAgedClient.ViewModel
{
    /// <summary>
    /// 用于接收显示远端的火柴人图像
    /// 需要设置RoomId,是需要打开图像显示的房间Id
    /// 需要设置Url1,Url2,Url3,Url4,分别是CameraId，即指定哪个摄像头显示在哪个Image框中
    /// </summary>
    public class HuoChaiRenFourVideoVM : BaseFourVideoVM
    {
        public UInt32 RoomId { get; set; }

        private TcpClient tcpClient;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSource;
        private readonly int remote_port = 8008;
        private string remote_ip = "127.0.0.1";

        private async Task<byte[]> tcp_recv(NetworkStream stream, int data_len)
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

            remote_ip = ConfigurationManager.AppSettings["TcpServerIp"];
            tcpClient = new TcpClient(remote_ip, remote_port);
            Console.WriteLine($"tcp connect: ({remote_ip}, {remote_port})");

            stream = tcpClient.GetStream();

            //发送角色数据包
            Role role = new Role(3, 1, 1);
            byte[] send_bytes = StructToBytes(role);
            stream.WriteAsync(send_bytes, 0, send_bytes.Length);
            Console.WriteLine("send_role_packet");

            //发送获取图像请求命令包
            VideoCmd videoCmd = new VideoCmd(1, 5, RoomId, 1);
            byte[] videoCmd_bytes = StructToBytes<VideoCmd>(videoCmd);
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
        }

        private byte[] StructToBytes<T>(T videoCmd)
        {
            throw new NotImplementedException();
        }

        private async Task RecvVideo(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                byte[] video_header_bytes = await tcp_recv(stream, 9);
                VideoHeader videoHeader = BytesToStruct<VideoHeader>(video_header_bytes);
                if (videoHeader.type == 2)
                {
                    byte[] video_image_bytes = await tcp_recv(stream, (int)(videoHeader.len) - 4);
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

        private T BytesToStruct<T>(byte[] video_header_bytes)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            //发送关闭图像命令
            //VideoCmd videoCmd = new VideoCmd(1, 5, RoomId, 0);
            //byte[] videoCmd_bytes = StructToBytesHelper.StructToBytes<VideoCmd>(videoCmd);
            //stream.Write(videoCmd_bytes, 0, videoCmd_bytes.Length);

            cancellationTokenSource.Cancel();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Role
    {
        public byte type;
        public UInt32 len;
        public byte role;

        public Role(byte type, UInt32 len, byte role)
        {
            this.type = type;
            this.len = len;
            this.role = role;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VideoCmd
    {
        public byte type;
        public UInt32 len;
        public UInt32 roomId;
        public byte isOpen;

        public VideoCmd(byte type, UInt32 len, UInt32 roomId, byte isOpen)
        {
            this.type = type;
            this.len = len;
            this.roomId = roomId;
            this.isOpen = isOpen;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VideoHeader
    {
        public byte type;
        public UInt32 len;
        public UInt32 cameraId;

        public VideoHeader(byte type, UInt32 len, UInt32 cameraId)
        {
            this.type = type;
            this.len = len;
            this.cameraId = cameraId;
        }
    }
}