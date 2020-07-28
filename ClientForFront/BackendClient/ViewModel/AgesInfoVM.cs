using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class AgesInfoVM:ViewModelBase
    {
        private HttpClient httpClient;
        public ObservableCollection<AgesInfo> _agesinfoes = new ObservableCollection<AgesInfo>();
        public ObservableCollection<AgesInfo> AgesInfoes
        {
            get => _agesinfoes;
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
        public AgesInfoVM(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private void Loaded()
        {
            LogHelper.Debug("AgesInfoView Loaded.");
            GetAgedsAsync();
        }

        private void Unloaded()
        {
            LogHelper.Debug("AgesInfoView UnLoaded.");
        }

        private async void GetAgedsAsync()
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
                LogHelper.Debug($"GetAgeds caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var ageds = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
                //检查有无新增
                foreach (var item in ageds)
                {
                    if (!AgesInfoes.Any(x => x.Id == item.Id))
                    {
                        AgesInfoes.Add(item);
                    }
                }
                //检查有无删减
                for (int i = AgesInfoes.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in ageds)
                    {
                        if (AgesInfoes.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        AgesInfoes.RemoveAt(i);
                    }
                }
            }
        }
    }
}
