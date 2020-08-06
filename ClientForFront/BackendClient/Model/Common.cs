using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackendClient.Model
{
    public class Common
    {
        public static async Task<bool> DelItem(HttpClient httpClient, string url,long id)
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

        public static void ShowWindow(Window view,bool isNew,Action actionAfterClose=null)
        {
            view.Owner = App.Current.MainWindow;
            view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            view.ShowDialog();

            if (isNew)//新建
            {
                if (view.DialogResult == true)//新建成功，刷新显示
                {
                    actionAfterClose();
                }
            }
        }
    }
}
