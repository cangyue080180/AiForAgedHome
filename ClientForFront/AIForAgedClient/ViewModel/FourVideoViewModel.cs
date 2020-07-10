using AIForAgedClient.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AIForAgedClient.ViewModel
{
    public class FourVideoViewModel:ViewModelBase
    {
        #region 设置Image Source属性
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
        #endregion

        #region 设置Image RowSpan 和 ColumnSpan属性
        private int _img1RowSpan=1;
        public int Img1RowSpan
        {
            get
            {
                return _img1RowSpan;
            }
            set
            {
                if (_img1RowSpan != value)
                {
                    _img1RowSpan = value;
                    RaisePropertyChanged(nameof(Img1RowSpan));
                }
            }
        }

        private int _img1ColumnSpan=1;
        public int Img1ColumnSpan
        {
            get
            {
                return _img1ColumnSpan;
            }
            set
            {
                if (_img1ColumnSpan != value)
                {
                    _img1ColumnSpan = value;
                    RaisePropertyChanged(nameof(Img1ColumnSpan));
                }
            }
        }

        private int _img2RowSpan = 1;
        public int Img2RowSpan
        {
            get
            {
                return _img2RowSpan;
            }
            set
            {
                if (_img2RowSpan != value)
                {
                    _img2RowSpan = value;
                    RaisePropertyChanged(nameof(Img2RowSpan));
                }
            }
        }

        private int _img2ColumnSpan = 1;
        public int Img2ColumnSpan
        {
            get
            {
                return _img2ColumnSpan;
            }
            set
            {
                if (_img2ColumnSpan != value)
                {
                    _img2ColumnSpan = value;
                    RaisePropertyChanged(nameof(Img2ColumnSpan));
                }
            }
        }

        private int _img3RowSpan = 1;
        public int Img3RowSpan
        {
            get
            {
                return _img3RowSpan;
            }
            set
            {
                if (_img3RowSpan != value)
                {
                    _img3RowSpan = value;
                    RaisePropertyChanged(nameof(Img3RowSpan));
                }
            }
        }

        private int _img3ColumnSpan = 1;
        public int Img3ColumnSpan
        {
            get
            {
                return _img3ColumnSpan;
            }
            set
            {
                if (_img3ColumnSpan != value)
                {
                    _img3ColumnSpan = value;
                    RaisePropertyChanged(nameof(Img3ColumnSpan));
                }
            }
        }

        private int _img4RowSpan = 1;
        public int Img4RowSpan
        {
            get
            {
                return _img4RowSpan;
            }
            set
            {
                if (_img4RowSpan != value)
                {
                    _img4RowSpan = value;
                    RaisePropertyChanged(nameof(Img4RowSpan));
                }
            }
        }

        private int _img4ColumnSpan = 1;
        public int Img4ColumnSpan
        {
            get
            {
                return _img4ColumnSpan;
            }
            set
            {
                if (_img4ColumnSpan != value)
                {
                    _img4ColumnSpan = value;
                    RaisePropertyChanged(nameof(Img4ColumnSpan));
                }
            }
        }
        #endregion

        #region 设置Image Visibility属性
        private Visibility _img1Visibility=Visibility.Visible;
        private Visibility Img1Visibility
        {
            get
            {
                return _img1Visibility;
            }
            set
            {
                if (_img1Visibility != value)
                {
                    _img1Visibility = value;
                    RaisePropertyChanged(nameof(Img1Visibility));
                }
            }
        }

        private Visibility _img2Visibility = Visibility.Visible;
        private Visibility Img2Visibility
        {
            get
            {
                return _img2Visibility;
            }
            set
            {
                if (_img2Visibility != value)
                {
                    _img2Visibility = value;
                    RaisePropertyChanged(nameof(Img2Visibility));
                }
            }
        }

        private Visibility _img3Visibility = Visibility.Visible;
        private Visibility Img3Visibility
        {
            get
            {
                return _img3Visibility;
            }
            set
            {
                if (_img3Visibility != value)
                {
                    _img3Visibility = value;
                    RaisePropertyChanged(nameof(Img3Visibility));
                }
            }
        }

        private Visibility _img4Visibility = Visibility.Visible;
        private Visibility Img4Visibility
        {
            get
            {
                return _img4Visibility;
            }
            set
            {
                if (_img4Visibility != value)
                {
                    _img4Visibility = value;
                    RaisePropertyChanged(nameof(Img4Visibility));
                }
            }
        }
        #endregion

        #region 设置Image doubleClick 命令
        private RelayCommand _img1Dbclick;
        public ICommand Img1DoubleClickCmd
        {
            get
            {
                if (_img1Dbclick == null)
                    _img1Dbclick = new RelayCommand(()=> { Img1DoubleClick(); });
                return _img1Dbclick;
            }
        }

        private RelayCommand _img2Dbclick;
        public ICommand Img2DoubleClickCmd
        {
            get
            {
                if (_img2Dbclick == null)
                    _img2Dbclick = new RelayCommand(() => { Img2DoubleClick(); });
                return _img2Dbclick;
            }
        }

        private RelayCommand _img3Dbclick;
        public ICommand Img3DoubleClickCmd
        {
            get
            {
                if (_img3Dbclick == null)
                    _img3Dbclick = new RelayCommand(() => { Img3DoubleClick(); });
                return _img3Dbclick;
            }
        }

        private RelayCommand _img4Dbclick;
        public ICommand Img4DoubleClickCmd
        {
            get
            {
                if (_img4Dbclick == null)
                    _img4Dbclick = new RelayCommand(() => { Img4DoubleClick(); });
                return _img4Dbclick;
            }
        }
        #endregion

        private readonly VideoPlayHelper videoPlayHelper1;
        private readonly VideoPlayHelper videoPlayHelper2;
        private readonly VideoPlayHelper videoPlayHelper3;
        private readonly VideoPlayHelper videoPlayHelper4;

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

        private void Img1DoubleClick()
        {
            System.Console.WriteLine("Image1 Double click.");
            
            if (Img1ColumnSpan == 1)
            {
                Img2Visibility = Visibility.Collapsed;
                Img3Visibility = Visibility.Collapsed;
                Img4Visibility = Visibility.Collapsed;
                Img1ColumnSpan = 2;
                Img1RowSpan = 2;
            }
            else
            {
                Img2Visibility = Visibility.Visible;
                Img3Visibility = Visibility.Visible;
                Img4Visibility = Visibility.Visible;
                Img1RowSpan = 1;
                Img1ColumnSpan = 1;
            }
        }

        private void Img2DoubleClick()
        {
            System.Console.WriteLine("Image2 Double click.");
            if (Img2ColumnSpan == 1)
            {
                Img1Visibility = Visibility.Collapsed;
                Img3Visibility = Visibility.Collapsed;
                Img4Visibility = Visibility.Collapsed;
                Img2ColumnSpan = 2;
                Img2RowSpan = 2;
            }
            else
            {
                Img1Visibility = Visibility.Visible;
                Img3Visibility = Visibility.Visible;
                Img4Visibility = Visibility.Visible;
                Img2RowSpan = 1;
                Img2ColumnSpan = 1;
            }
        }

        private void Img3DoubleClick()
        {
            System.Console.WriteLine("Image3 Double click.");
            if (Img3ColumnSpan == 1)
            {
                Img1Visibility = Visibility.Collapsed;
                Img2Visibility = Visibility.Collapsed;
                Img4Visibility = Visibility.Collapsed;
                Img3ColumnSpan = 2;
                Img3RowSpan = 2;
            }
            else
            {
                Img1Visibility = Visibility.Visible;
                Img2Visibility = Visibility.Visible;
                Img4Visibility = Visibility.Visible;
                Img3RowSpan = 1;
                Img3ColumnSpan = 1;
            }
        }

        private void Img4DoubleClick()
        {
            System.Console.WriteLine("Image4 Double click.");
            if (Img4ColumnSpan == 1)
            {
                Img1Visibility = Visibility.Collapsed;
                Img3Visibility = Visibility.Collapsed;
                Img2Visibility = Visibility.Collapsed;
                Img4ColumnSpan = 2;
                Img4RowSpan = 2;
            }
            else
            {
                Img1Visibility = Visibility.Visible;
                Img3Visibility = Visibility.Visible;
                Img2Visibility = Visibility.Visible;
                Img4RowSpan = 1;
                Img4ColumnSpan = 1;
            }
        }
    }
}
