using AutoMapper;
using BackendClient.Model;
using BackendClient.View;
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

namespace BackendClient.ViewModel
{
    public class UserInfoDatasVM : DatasVMBase<UserInfoVM>
    {
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public UserInfoDatasVM(HttpClient httpClient, Mapper mapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = mapper;
        }

        public override void Change(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as UserInfoVM;
            SelectedItem = item;
            SimpleIoc.Default.Register(() => SelectedItem);
            Common.ShowWindow(new NewUser(), false, null);
        }

        public async override Task Delete(Hyperlink hyperlink)
        {
            var item = hyperlink.DataContext as UserInfoVM;
            SelectedItem = item;

            bool result = await Common.DelItem(httpClient, ConfigurationManager.AppSettings["GetUserInfoUrl"], item.Id);
            if (result)
            {
                ItemsSource.Remove(SelectedItem);
            }
        }

        public override void Loaded()
        {
            LogHelper.Debug("UserInfoView Loaded.");
            Update();
        }

        public override void New()
        {
            Common.ShowWindow(new NewUser(), true, Update);
        }

        public override void Search(string content)
        {
            ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
            if (!string.IsNullOrEmpty(content))
            {
                listCollectionView.Filter = (obj) =>
                {
                    UserInfoVM tempInfoVM = obj as UserInfoVM;
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
            LogHelper.Debug("UserInfoView UnLoaded.");
        }

        public override void Update()
        {
            UpdateSourceAsync(ItemsSource);
        }

        private async void UpdateSourceAsync(IList<UserInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetUserInfoUrl"];
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
                var sourceCollection = JsonConvert.DeserializeObject<List<UserInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<UserInfoVM>(item));
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