using AutoMapper;
using BackendClient.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using DataModel;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace BackendClient.ViewModel
{
    public class NewAgedVM
    {
        //当为true时是新建对象，为false时是修改对象
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        public ObservableCollection<RoomInfoVM> Rooms { get; } = new ObservableCollection<RoomInfoVM>();

        public RoomInfoVM SelectedRoom { get; set; }

        public Visibility IsNew
        {
            get
            {
                if (isNew)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public string Title { get; set; }
        public AgesInfoVM AgesInfoVM { get; }

        private RelayCommand _onLoadCmd;
        public ICommand OnLoadCmd
        {
            get
            {
                if (_onLoadCmd == null)
                {
                    _onLoadCmd = new RelayCommand(() =>
                    {
                        OnWindowLoaded();
                    });
                }
                return _onLoadCmd;
            }
        }

        private RelayCommand _onClosingCmd;
        public ICommand OnClosingCmd
        {
            get
            {
                if (_onClosingCmd == null)
                {
                    _onClosingCmd = new RelayCommand(() =>
                    {
                        OnWindowClosing();
                    });
                }
                return _onClosingCmd;
            }
        }

        private RelayCommand<Window> _okCmd;
        public RelayCommand<Window> OkCmd
        {
            get
            {
                if (_okCmd == null)
                {
                    _okCmd = new RelayCommand<Window>(
                       async win => {
                           bool result = await Ok();
                           if (result)
                           {
                               win.DialogResult = true;
                           }
                           win.Close();
                       });
                }
                return _okCmd;
            }
        }

        private RelayCommand<Window> _cancelCmd;
        public RelayCommand<Window> CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand<Window>(win => { win.Close(); });
                }
                return _cancelCmd;
            }
        }

        public NewAgedVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<AgesInfoVM>())//修改
            {
                isNew = false;
                AgesInfoVM = SimpleIoc.Default.GetInstance<AgesInfoVM>();
                Title = "修改老人信息";
                Rooms.Add(AgesInfoVM.RoomInfo);
                SelectedRoom = AgesInfoVM.RoomInfo;
            }
            else
            {
                isNew = true;
                AgesInfoVM = new AgesInfoVM();
                Title = "创建新人员";
            }
        }

        private async Task<bool> Ok()
        {
            if (isNew)
            {
                AgesInfoVM.RoomInfoId = SelectedRoom.Id;
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetAgedsUrl"], autoMapper.Map<AgesInfo>(AgesInfoVM));
            }
            else
            {
                AgesInfoVM.RoomInfoId = SelectedRoom.Id;
                AgesInfoVM.RoomInfo = SelectedRoom;
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetAgedsUrl"], AgesInfoVM.Id, autoMapper.Map<AgesInfo>(AgesInfoVM));
            }
        }

        private void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewAgedWindowLoaded()");
            UpdateSourceAsync(Rooms);
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug("OnNewAgedWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<AgesInfoVM>();
            }
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
