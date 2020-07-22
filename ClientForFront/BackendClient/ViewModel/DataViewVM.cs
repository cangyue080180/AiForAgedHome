using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Windows.Documents;
using System.Windows.Input;
using Newtonsoft.Json;

namespace BackendClient.ViewModel
{
    public class DataViewVM : ViewModelBase
    {
        private HttpClient httpClient;
        #region property and command
        private List<AgesInfo> _ageds = new List<AgesInfo>();
        public List<AgesInfo> Ageds
        {
            get
            {
                return _ageds;
            }
            set
            {
                if (value != _ageds)
                {
                    _ageds = value;
                    RaisePropertyChanged(nameof(Ageds));
                }
            }
        }

        public AgesInfo SelectedAged
        {
            get;set;
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

        private RelayCommand _userSelectedChangedCmd;
        public ICommand UserSelectionChangeCmd
        {
            get
            {
                if (_userSelectedChangedCmd == null)
                    _userSelectedChangedCmd = new RelayCommand(OnUserSelectionChanged);
                return _userSelectedChangedCmd;
            }
        }
        #endregion

        private void Loaded()
        {
            LogHelper.Debug("DataView Loaded.");

            httpClient = MainViewModel.httpClient;
            GetAgedsAsync();
        }

        private async void GetAgedsAsync()
        {
            string url = ConfigurationManager.AppSettings["GetAgedsUrl"];
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
                Ageds = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
            }
        }

        //选择用户改变事件
        private void OnUserSelectionChanged()
        {
            LogHelper.Debug($"selected aged: {SelectedAged.Name}");

            string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];

        }

        private void Unloaded()
        {
            LogHelper.Debug("DataView Unloaded.");
        }
    }
}
