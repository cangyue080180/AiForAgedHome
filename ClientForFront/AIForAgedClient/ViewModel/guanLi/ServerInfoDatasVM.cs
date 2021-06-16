using AIForAgedClient.Helper;
using AIForAgedClient.Model;
using AIForAgedClient.View;
using AutoMapper;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;

namespace AIForAgedClient.ViewModel
{
    public class ServerInfoDatasVM : DatasVMBase<ServerInfoVM>
    {
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public ServerInfoDatasVM(HttpClient httpClient, Mapper mapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = mapper;
        }

        public override void Change(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as ServerInfoVM;
            SelectedItem = item;
            SimpleIoc.Default.Register(() => SelectedItem);
            Common.ShowWindow(new NewServer(), false, null);
        }

        public async override Task Delete(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as ServerInfoVM;
            SelectedItem = item;

            bool result = await Common.DelItem(httpClient, ConfigurationManager.AppSettings["GetServerInfoUrl"], item.Id);
            if (result)
            {
                ItemsSource.Remove(SelectedItem);
            }
        }

        public override void Loaded()
        {
            LogHelper.Debug("ServerInfoView Loaded.");
            Update();
        }

        public override void Unloaded()
        {
            LogHelper.Debug("ServerInfoView UnLoaded.");
        }

        public override void New()
        {
            Common.ShowWindow(new NewServer(), true, Update);
        }

        public override void Update()
        {
            UpdateSourceAsync(ItemsSource);
        }

        private async void UpdateSourceAsync(IList<ServerInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetServerInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetServerInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var sourceCollection = JsonConvert.DeserializeObject<List<ServerInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<ServerInfoVM>(item));
                    }
                    else
                    {
                        autoMapper.Map(item, exitInfo);
                    }
                }
                //检查有无删减
                for (int i = targetCollection.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in sourceCollection)
                    {
                        if (targetCollection.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        targetCollection.RemoveAt(i);
                    }
                }
            }
        }

        public override void Search(string content)
        {
            ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
            if (!string.IsNullOrEmpty(content))
            {
                listCollectionView.Filter = (obj) =>
                {
                    ServerInfoVM tempInfoVM = obj as ServerInfoVM;
                    return (tempInfoVM.Name == content);
                };
            }
            else
            {
                listCollectionView.Filter = null;
            }
        }
    }
}