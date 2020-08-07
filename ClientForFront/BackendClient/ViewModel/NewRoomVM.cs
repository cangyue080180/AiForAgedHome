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
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
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
        public RoomInfoVM RoomInfoVM { get; }

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
                            bool result= await Ok();
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
                RoomInfoVM = SimpleIoc.Default.GetInstance<RoomInfoVM>();
                Title = "修改房间信息";
            }
            else
            {
                isNew = true;
                RoomInfoVM = new RoomInfoVM();
                Title = "创建新房间";
            }
        }

        private async Task<bool> Ok()
        {
            if (isNew)
            {
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetRoomInfoUrl"], autoMapper.Map<RoomInfo>(RoomInfoVM));
            }
            else
            {
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetRoomInfoUrl"],RoomInfoVM.Id, autoMapper.Map<RoomInfo>(RoomInfoVM));
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
