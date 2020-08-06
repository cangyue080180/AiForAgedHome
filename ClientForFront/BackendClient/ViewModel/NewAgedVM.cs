using AutoMapper;
using BackendClient.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class NewAgedVM
    {
        //当为true时是新建对象，为false时是修改对象
        private bool isNew = true;
        private HttpClient httpClient;
        private IMapper autoMapper;
        public Visibility IsNew
        {
            get
            {
                if (isNew)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public string Title { get; set; }

        private AgesInfoVM _agesInfoVM;
        public AgesInfoVM AgesInfoVM
        {
            get => _agesInfoVM;
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

        private RelayCommand<Window> _okCmd;
        public RelayCommand<Window> OkCmd
        {
            get
            {
                if (_okCmd == null)
                {
                    _okCmd = new RelayCommand<Window>(win => { Ok(); win.DialogResult = true; win.Close(); });
                }
                return _okCmd;
            }
        }

        private RelayCommand<Window> _cancelCmd;
        public RelayCommand<Window> CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand<Window>(win => { win.Close(); });
                }
                return _cancelCmd;
            }
        }

        public NewAgedVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<AgesInfoVM>())//修改
            {
                isNew = false;
                _agesInfoVM = SimpleIoc.Default.GetInstance<AgesInfoVM>();
                Title = "修改老人信息";
            }
            else
            {
                isNew = true;
                _agesInfoVM = new AgesInfoVM();
                Title = "创建新人员";
            }
        }

        private void Ok()
        {
            if (isNew)
            {
                PostNew();
            }
            else
            {
                Put();
            }
        }
        private void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewAgedWindowLoaded()");
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug("OnNewAgedWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<AgesInfoVM>();
            }
        }

        private void Put()
        {
            throw new NotImplementedException();
        }

        private void PostNew()
        {
            throw new NotImplementedException();
        }
    }
}
