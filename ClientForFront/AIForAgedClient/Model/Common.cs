using AIForAgedClient.Helper;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AIForAgedClient.Model
{
    public class Common
    {
        //新建
        public static async Task<bool> PostNew(HttpClient httpClient, string url, object obj)
        {
            string jsonResult = JsonConvert.SerializeObject(obj);
            HttpContent httpContent = new StringContent(jsonResult);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(url, httpContent);
                return true;
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug("Post exception. " + e.Message);
            }
            catch (ArgumentNullException)
            {
            }
            return false;
        }

        //删除
        public static async Task<bool> DelItem(HttpClient httpClient, string url, long id)
        {
            try
            {
                var result = await httpClient.DeleteAsync(url + $"/{id}");
                return true;
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"Delete Item caught exception: {e.Message}");
                return false;
            }
        }

        //修改
        public static async Task<bool> Put(HttpClient httpClient, string url, long id, object obj)
        {
            url += $"/{id}";

            string jsonResult = JsonConvert.SerializeObject(obj);
            HttpContent httpContent = new StringContent(jsonResult);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PutAsync(url, httpContent);
                return true;
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug("PutRoom exception. " + e.Message);
            }
            catch (ArgumentNullException)
            {
            }
            return false;
        }

        //显示子窗体
        public static void ShowWindow(Window view, bool isNew, Action actionAfterClose = null)
        {
            view.Owner = App.Current.MainWindow;
            view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            view.ShowDialog();

            if (isNew)//新建
            {
                if (view.DialogResult == true)//在子窗体按确定并执行成功，刷新显示
                {
                    actionAfterClose();
                }
            }
        }
    }
}