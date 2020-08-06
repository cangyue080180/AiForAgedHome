using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class NewRoomVM
    {
        //当为true时是新建对象，为false时是修改对象
        private bool isNew = true;
        private HttpClient httpClient;
        private IMapper autoMapper;
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

        private RoomInfoVM roomInfoVM=null;
        public RoomInfoVM RoomInfoVM
        {
            get => roomInfoVM;
        }

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
                    _okCmd = new RelayCommand<Window>(win => { Ok();win.DialogResult = true; win.Close();  });
                }
                return _okCmd;
            }
        }

        private RelayCommand<Window> _cancelCmd;
        public RelayCommand<Window> CancelCmd
        {
            get
            {
                if(_cancelCmd==null)
                {
                    _cancelCmd = new RelayCommand<Window>(win=> { win.Close(); });
                }
                return _cancelCmd;
            }
        }

        public NewRoomVM(HttpClient httpClient,Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<RoomInfoVM>())//修改
            {
                isNew = false;
                roomInfoVM = SimpleIoc.Default.GetInstance<RoomInfoVM>();
                Title = "修改房间信息";
            }
            else
            {
                isNew = true;
                roomInfoVM = new RoomInfoVM();
                Title = "创建新房间";
            }
        }

        private void Ok()
        {
            if (isNew)
            {
                PostNewRoom();
            }
            else
            {
                PutRoom();
            }
        }

        //TODO 将下列方法提取出到Common中去。
        //修改
        private async void PutRoom()
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            url += $"/{roomInfoVM.Id}";

            string jsonResult = JsonConvert.SerializeObject(autoMapper.Map<RoomInfo>(roomInfoVM));
            HttpContent httpContent = new StringContent(jsonResult);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PutAsync(url, httpContent);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug("PutRoom exception. "+e.Message);
            }
            catch (ArgumentNullException)
            {

            }
        }
        //新建
        private async void PostNewRoom()
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];

            string jsonResult = JsonConvert.SerializeObject(autoMapper.Map<RoomInfo>(roomInfoVM));
            HttpContent httpContent = new StringContent(jsonResult);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(url, httpContent);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug("PostRoom exception. "+e.Message);
            }
            catch (ArgumentNullException)
            {

            }
        }

        private void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewRoomWindowLoaded()");
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug("OnNewRoomWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<RoomInfoVM>();
            }
        }
    }
}
