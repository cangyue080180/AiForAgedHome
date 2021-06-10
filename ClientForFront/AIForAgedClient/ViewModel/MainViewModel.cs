using AIForAgedClient.Helper;
using AIForAgedClient.View;
using AutoMapper;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
        }

        private void OnWindowLoaded()
        {
            ContentViewModel = SimpleIoc.Default.GetInstance<PoseInfoesVM>();
        }

        private void OnWindowClosing()
        {
            // dispatcherTimer.Stop();
        }
    }
}