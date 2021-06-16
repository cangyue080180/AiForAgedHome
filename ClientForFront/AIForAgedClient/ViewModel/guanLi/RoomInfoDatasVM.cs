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
    public class RoomInfoDatasVM : DatasVMBase<RoomInfoVM>
    {
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public RoomInfoDatasVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;
        }

        public override void Change(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as RoomInfoVM;
            SelectedItem = item;
            SimpleIoc.Default.Register(() => SelectedItem);
            Common.ShowWindow(new NewRoom(), false, null);
        }

        public async override Task Delete(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as RoomInfoVM;
            SelectedItem = item;

            bool result = await Common.DelItem(httpClient, ConfigurationManager.AppSettings["GetRoomInfoUrl"], item.Id);
            if (result)
            {
                ItemsSource.Remove(SelectedItem);
            }
        }

        public override void Loaded()
        {
            LogHelper.Debug("RoomInfoView Loaded.");
            Update();
        }

        public override void New()
        {
            Common.ShowWindow(new NewRoom(), true, Update);
        }

        public override void Search(string content)
        {
            ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
            if (!string.IsNullOrEmpty(content))
            {
                listCollectionView.Filter = (obj) =>
                {
                    RoomInfoVM tempInfoVM = obj as RoomInfoVM;
                    return (tempInfoVM.Name == content);
                };
            }
            else
            {
                listCollectionView.Filter = null;
            }
        }

        public override void Unloaded()
        {
            LogHelper.Debug("RoomInfoView UnLoaded.");
        }

        public override void Update()
        {
            UpdateSourceAsync(ItemsSource);
        }

        private async void UpdateSourceAsync(IList<RoomInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetAgesInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var sourceCollection = JsonConvert.DeserializeObject<List<RoomInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<RoomInfoVM>(item));
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
    }
}