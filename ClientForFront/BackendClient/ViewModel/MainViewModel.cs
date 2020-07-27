using BackendClient.Model;
using BackendClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Net.Http;
using System.Reflection;
using System.Windows.Input;

namespace BackendClient.ViewModel
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
        public static readonly HttpClient httpClient = new HttpClient();

        public string Title
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private RelayCommand _aboutCmd;
        public ICommand AboutCmd
        {
            get
            {
                if (_aboutCmd == null)
                    _aboutCmd = new RelayCommand(() =>
                    {
                        HelpWindow helpWindow = new HelpWindow();
                        helpWindow.Owner = App.Current.MainWindow;
                        helpWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                        helpWindow.Show();
                    });
                return _aboutCmd;
            }
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

        private void OnWindowLoaded()
        {
            LogHelper.Debug("OnMainWindowLoaded()");
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug("OnMainWindowClosing()");
        }
    }
}