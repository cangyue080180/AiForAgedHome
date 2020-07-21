using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class DataViewVM : ViewModelBase
    {
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

        private void Loaded()
        {
            System.Console.WriteLine("DataView Loaded.");
            Ageds = new List<AgesInfo>()
            {
                new AgesInfo()
                {
                    Name="age1"
                },
                new AgesInfo()
                {
                    Name="age2"
                }
            };
        }

        private void Unloaded()
        {
            System.Console.WriteLine("DataView Unloaded.");
        }
    }
}
