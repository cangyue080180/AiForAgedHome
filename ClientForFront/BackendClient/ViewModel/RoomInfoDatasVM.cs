﻿using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Windows.Documents;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class RoomInfoDatasVM : ViewModelBase
    {
        private HttpClient httpClient;
        private IMapper autoMapper;
        public ObservableCollection<RoomInfoVM> _roominfoes = new ObservableCollection<RoomInfoVM>();
        public ObservableCollection<RoomInfoVM> RoomInfoes
        {
            get => _roominfoes;
        }

        private RoomInfoVM _selectedItem;
        public RoomInfoVM SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem,value);
        }

        private RelayCommand _onLoadedCmd;
        public ICommand OnLoadedCmd
        {
            get
            {
                if (_onLoadedCmd == null)
                    _onLoadedCmd = new RelayCommand(Loaded);
                return _onLoadedCmd;
            }
        }

        private RelayCommand _onUnloadedCmd;
        public ICommand OnUnloadedCmd
        {
            get
            {
                if (_onUnloadedCmd == null)
                    _onUnloadedCmd = new RelayCommand(Unloaded);
                return _onUnloadedCmd;
            }
        }
        private RelayCommand _updateCmd;
        public ICommand UpdateCmd
        {
            get
            {
                if (_updateCmd == null)
                    _updateCmd = new RelayCommand(()=>{ GetRoomInfoesAsync(); });
                return _updateCmd;
            }
        }

        private RelayCommand<Hyperlink> _delCmd;
        public ICommand DelCmd
        {
            get
            {
                if(_delCmd==null)
                {
                    _delCmd = new RelayCommand<Hyperlink>(x=> {
                        var item = x.DataContext as RoomInfoVM;
                        SelectedItem = item;
                        DelItem();
                    });
                }
                return _delCmd;
            }
        }

        public RoomInfoDatasVM(HttpClient httpClient,Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;
        }

        private void Loaded()
        {
            LogHelper.Debug("RoomInfoView Loaded.");
            GetRoomInfoesAsync();
        }

        private void Unloaded()
        {
            LogHelper.Debug("RoomInfoView UnLoaded.");
        }

        private async void GetRoomInfoesAsync()
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetRoomInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var roomInfos = JsonConvert.DeserializeObject<List<RoomInfo>>(result);
                //检查有无新增
                foreach (var item in roomInfos)
                {
                    var roomInfo = RoomInfoes.FirstOrDefault(x => x.Id == item.Id);
                    if (roomInfo==null)
                    {
                        RoomInfoes.Add(autoMapper.Map<RoomInfoVM>(item));
                    }
                    else
                    {
                        autoMapper.Map(item,roomInfo);
                    }
                }
                //检查有无删减
                for (int i = RoomInfoes.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in roomInfos)
                    {
                        if (RoomInfoes.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        RoomInfoes.RemoveAt(i);
                    }
                }
            }
        }

        private void DelItem()
        {
            RoomInfoes.Remove(SelectedItem);
        }
    }
}