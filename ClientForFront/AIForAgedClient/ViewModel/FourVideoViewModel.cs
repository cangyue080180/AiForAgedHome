using AIForAgedClient.Helper;
using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;

namespace AIForAgedClient.ViewModel
{
    public class FourVideoViewModel:ViewModelBase
    {
        private BitmapImage _img1;
        public BitmapImage Image1
        {
            get
            {
                return _img1;
            }
            set
            {
                if (_img1 != value)
                {
                    _img1 = value;
                    RaisePropertyChanged(nameof(Image1));
                }
            }
        }

        private BitmapImage _img2;
        public BitmapImage Image2
        {
            get
            {
                return _img2;
            }
            set
            {
                if (_img2 != value)
                {
                    _img2 = value;
                    RaisePropertyChanged(nameof(Image2));
                }
            }
        }

        private BitmapImage _img3;
        public BitmapImage Image3
        {
            get
            {
                return _img3;
            }
            set
            {
                if (_img3 != value)
                {
                    _img3 = value;
                    RaisePropertyChanged(nameof(Image3));
                }
            }
        }

        private BitmapImage _img4;
        public BitmapImage Image4
        {
            get
            {
                return _img4;
            }
            set
            {
                if (_img4 != value)
                {
                    _img4 = value;
                    RaisePropertyChanged(nameof(Image4));
                }
            }
        }

        private readonly VideoPlayHelper videoPlayHelper1;
        private readonly VideoPlayHelper videoPlayHelper2;
        private readonly VideoPlayHelper videoPlayHelper3;
        private readonly VideoPlayHelper videoPlayHelper4;

        public FourVideoViewModel()
        {

        }

        public FourVideoViewModel(string url1,string url2=null,string url3=null,string url4=null)
        {
            if (!string.IsNullOrEmpty(url1))
            {
                videoPlayHelper1 = new VideoPlayHelper(url1, (x) => { Image1 = x; });
            }

            if (!string.IsNullOrEmpty(url2))
            {
                videoPlayHelper2 = new VideoPlayHelper(url2,(x)=> { Image2 = x; });
            }

            if (!string.IsNullOrEmpty(url3))
            {
                videoPlayHelper3 = new VideoPlayHelper(url3,(x)=> { Image3 = x; });
            }

            if (!string.IsNullOrEmpty(url4))
            {
                videoPlayHelper4 = new VideoPlayHelper(url4,(x)=> { Image4 = x; });
            }
        }

        public void Start()
        {
            videoPlayHelper1?.Start();
            videoPlayHelper2?.Start();
            videoPlayHelper3?.Start();
            videoPlayHelper4?.Start();
        }

        public void Stop()
        {
            videoPlayHelper1?.Stop();
            videoPlayHelper2?.Stop();
            videoPlayHelper3?.Stop();
            videoPlayHelper4?.Stop();
        }
    }
}
