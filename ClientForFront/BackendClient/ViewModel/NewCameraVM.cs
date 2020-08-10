using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendClient.ViewModel
{
    public class NewCameraVM : NewModelVMBase<CameraInfoVM>
    {
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public ObservableCollection<RoomInfoVM> Rooms { get; } = new ObservableCollection<RoomInfoVM>();
        public ObservableCollection<ServerInfoVM> ServerInfoes { get; } = new ObservableCollection<ServerInfoVM>();

        public RoomInfoVM SelectedRoom { get; set; }
        public ServerInfoVM SelectedServerInfo { get; set; }
        public NewCameraVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<CameraInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<CameraInfoVM>();
                Title = "修改摄像头信息";
                Rooms.Add(Model.RoomInfo);
                ServerInfoes.Add(Model.ServerInfo);
                SelectedRoom = Model.RoomInfo;
                SelectedServerInfo = Model.ServerInfo;
            }
            else
            {
                isNew = true;
                Model = new CameraInfoVM();
                Title = "创建新摄像头";
            }
        }
        public override bool IsNewModel()
        {
            return isNew;
        }

        public async override Task<bool> Ok()
        {
            if (isNew)
            {
                Model.RoomInfoId = SelectedRoom.Id;
                Model.ServerInfoId = SelectedServerInfo.Id;
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetCameraInfoUrl"], autoMapper.Map<CameraInfo>(Model));
            }
            else
            {
                Model.RoomInfoId = SelectedRoom.Id;
                Model.ServerInfoId = SelectedServerInfo.Id;
                Model.RoomInfo = SelectedRoom;
                Model.ServerInfo = SelectedServerInfo;
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetCameraInfoUrl"], Model.Id, autoMapper.Map<CameraInfo>(Model));
            }
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewAgedWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<CameraInfoVM>();
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewAgedWindowLoaded()");
            UpdateRoomsAsync(Rooms);
            UpdateServerInfoesAsync(ServerInfoes);
        }

        private async void UpdateServerInfoesAsync(IList<ServerInfoVM> targetCollection)
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

        private async void UpdateRoomsAsync(IList<RoomInfoVM> targetCollection)
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
