using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public abstract class DatasVMBase<T> : ViewModelBase
    {
        public ObservableCollection<T> ItemsSource { get; } = new ObservableCollection<T>();

        private T _selectedItem;
        public T SelectedItem
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
                    _newCmd = new RelayCommand(() =>
                    {
                        New();
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
                    _delCmd = new RelayCommand<Hyperlink>(async x =>
                    {
                        await Delete(x);
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
                    _changeCmd = new RelayCommand<Hyperlink>(x =>
                    {
                        Change(x);
                    });
                }
                return _changeCmd;
            }
        }

        public abstract void Loaded();

        public abstract void Unloaded();

        public abstract void Update();

        public abstract void Change(Hyperlink hyperlink);

        public abstract Task Delete(Hyperlink hyperlink);

        public abstract void New();
    }
}
