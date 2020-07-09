using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
        public FourVideoViewModel FourVideoVM
        {
            get;set;
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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            FourVideoVM = new FourVideoViewModel("http://ivi.bupt.edu.cn/hls/cctv5phd.m3u8",
                "http://ivi.bupt.edu.cn/hls/cctv1hd.m3u8",
                "http://ivi.bupt.edu.cn/hls/cctv2hd.m3u8",
                "http://ivi.bupt.edu.cn/hls/cctv8hd.m3u8");
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private void OnWindowLoaded()
        {
            FourVideoVM.Start();
        }

        private void OnWindowClosing()
        {
            FourVideoVM.Stop();
        }
    }
}