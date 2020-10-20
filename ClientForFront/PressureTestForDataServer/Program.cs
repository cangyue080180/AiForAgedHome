using DataModel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PressureTestForDataServer
{
    //测试
    internal class Program
    {
        private int writeTaskCount = 10;
        private int readTaskCount = 10;
        private static HttpClient httpClient = new HttpClient();
        private static bool isStop = false;

        private static void Main(string[] args)
        {
            Console.WriteLine("Pressure Test is Start.");
            Task.Run(() => { TestWriteDb(); });
            //Task.Run(() => { TestReadDb(); });
            Console.ReadKey();
        }

        //创建写数据库请求
        private static async void TestWriteDb()
        {
            string url = "https://localhost:44358/api/PoseInfoes";
            int[] agesIds = new int[] { 1, 2 };
            Random random = new Random();
            while (!isStop)
            {
                foreach (var item in agesIds)
                {
                    Task.Run(async () =>
                    {
                        PoseInfo poseInfo = new PoseInfo()
                        {
                            AgesInfoId = item,
                            Date = DateTime.Now.Date,
                            TimeDown = random.Next(1, 3600),
                            TimeIn = "08:35",
                            TimeLie = random.Next(1, 7200),
                            TimeOther = random.Next(1, 7200),
                            TimeSit = random.Next(1, 7300),
                            TimeStand = random.Next(1, 7200),
                            Status = (byte)random.Next(0, 5),
                            IsAlarm = false
                        };
                        string jsonResult = JsonConvert.SerializeObject(poseInfo);
                        HttpContent httpContent = new StringContent(jsonResult);
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        try
                        {
                            var result = await httpClient.PutAsync(url + $"/{item}", httpContent);
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine($"GetAgeds caught exception: {e.Message}");
                        }
                    });
                }
                await Task.Delay(2000);
            }

        }
        //创建读数据库请求
        private static async void TestReadDb()
        {

            string url = "https://localhost:44358/api/poseinfoes/getposeinfotoday";
            string result;
            while (!isStop)
            {
                try
                {
                    result = await httpClient.GetStringAsync(url);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"GetAgeds caught exception: {e.Message}");
                    result = null;
                }
                await Task.Delay(1000);
            }
        }
    }
}
