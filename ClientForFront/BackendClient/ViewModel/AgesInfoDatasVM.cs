using AutoMapper;
using BackendClient.Model;
using BackendClient.View;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class AgesInfoDatasVM : ViewModelBase
    {
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public ObservableCollection<AgesInfoVM> AgesInfoes { get; } = new ObservableCollection<AgesInfoVM>();

        private AgesInfoVM _selectedItem;
        public AgesInfoVM SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
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

        private RelayCommand _newCmd;
        public RelayCommand NewCmd//新建
        {
            get
            {
                if (_newCmd == null)
                {
                    _newCmd = new RelayCommand(() => {
                        Common.ShowWindow(new NewAged(), true,Update);
                    });
                }
                return _newCmd;
            }
        }

        private RelayCommand _updateCmd;
        public ICommand UpdateCmd//刷新显示
        {
            get
            {
                if (_updateCmd == null)
                    _updateCmd = new RelayCommand(() => { Update(); });
                return _updateCmd;
            }
        }

        private RelayCommand<Hyperlink> _delCmd;
        public ICommand DelCmd//删除
        {
            get
            {
                if (_delCmd == null)
                {
                    _delCmd = new RelayCommand<Hyperlink>(x => {
                        var item = x.DataContext as AgesInfoVM;
                        SelectedItem = item;
                        Task.Run(async () =>
                        {
                            bool result = await Common.DelItem(httpClient, ConfigurationManager.AppSettings["GetAgedsUrl"], item.Id);
                            if (result)
                            {
                                AgesInfoes.Remove(SelectedItem);
                            }
                        });
                    });
                }
                return _delCmd;
            }
        }

        private RelayCommand<Hyperlink> _changeCmd;
        public ICommand ChangeCmd
        {
            get
            {
                if (_changeCmd == null)
                {
                    _changeCmd = new RelayCommand<Hyperlink>(x => {
                        var item = x.DataContext as AgesInfoVM;
                        SelectedItem = item;
                        SimpleIoc.Default.Register(() => SelectedItem);
                        Common.ShowWindow(new NewAged(),false);
                    });
                }
                return _changeCmd;
            }
        }

        public AgesInfoDatasVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;
        }

        private void Loaded()
        {
            LogHelper.Debug("AgesInfoView Loaded.");
            Update();
        }

        private void Unloaded()
        {
            LogHelper.Debug("AgesInfoView UnLoaded.");
        }

        private void Update()
        {
            UpdateSourceAsync(AgesInfoes);
        }

        private async void UpdateSourceAsync(IList<AgesInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetAgedsUrl"];
            url += "/getagesinfoswithroominfo";
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
                var sourceCollection = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<AgesInfoVM>(item));
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
