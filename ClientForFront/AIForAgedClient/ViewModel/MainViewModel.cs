using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Reflection;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public string WindowName
        {
            get => Assembly.GetExecutingAssembly().GetName().Name;
        }

        private ViewModelBase _contentVM;

        public ViewModelBase ContentViewModel
        {
            get => _contentVM;
            set => Set(ref _contentVM, value);
        }

        private RelayCommand _onLoaded;

        public ICommand OnLoadedCommand
        {
            get
            {
                if (_onLoaded == null)
                    _onLoaded = new RelayCommand(OnWindowLoaded);
                return _onLoaded;
            }
        }

        private RelayCommand _onClosing;

        public ICommand OnClosingCommand
        {
            get
            {
                if (_onClosing == null)
                {
                    _onClosing = new RelayCommand(OnWindowClosing);
                }
                return _onClosing;
            }
        }

        private RelayCommand _monitorCommand;

        public ICommand MonitorCommand
        {
            get
            {
                if (_monitorCommand == null)
                {
                    _monitorCommand = new RelayCommand(OnMonitor);
                }
                return _monitorCommand;
            }
        }

        private void OnMonitor()
        {
            if (!SimpleIoc.Default.IsRegistered<PoseInfoesVM>())
            {
                SimpleIoc.Default.Register<PoseInfoesVM>();
            }
            ContentViewModel = SimpleIoc.Default.GetInstance<PoseInfoesVM>();
        }

        private RelayCommand _manageCommand;

        public ICommand ManageCommand
        {
            get
            {
                if (_manageCommand == null)
                {
                    _manageCommand = new RelayCommand(OnManage);
                }
                return _manageCommand;
            }
        }

        private void OnManage()
        {
            if (SimpleIoc.Default.IsRegistered<PoseInfoesVM>())
            {
                SimpleIoc.Default.Unregister<PoseInfoesVM>();
            }
            ContentViewModel = SimpleIoc.Default.GetInstance<DataManagerVM>();
        }

        private RelayCommand _showChartViewCommand;

        public ICommand ShowChartViewCommand
        {
            get
            {
                if (_showChartViewCommand == null)
                {
                    _showChartViewCommand = new RelayCommand(OnShowChartView);
                }
                return _showChartViewCommand;
            }
        }

        private void OnShowChartView()
        {
            ContentViewModel = SimpleIoc.Default.GetInstance<ChartViewVM>();
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
        }

        private void OnWindowLoaded()
        {
            SimpleIoc.Default.Register<PoseInfoesVM>();
            ContentViewModel = SimpleIoc.Default.GetInstance<PoseInfoesVM>();
        }

        private void OnWindowClosing()
        {
            // dispatcherTimer.Stop();
        }
    }
}