/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BackendClient"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AutoMapper;
using BackendClient.Model;
using CommonServiceLocator;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Net.Http;

namespace BackendClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            //SimpleIoc.Default.Register(()=>new HttpClient());
            SimpleIoc.Default.Register(() => new HttpClient());
            SimpleIoc.Default.Register(() => new Mapper(CreateConfiguration()));
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ChartViewVM>();
            SimpleIoc.Default.Register<HelpVM>();
            SimpleIoc.Default.Register<DataManagerVM>();
            SimpleIoc.Default.Register<RoomInfoDatasVM>();
            SimpleIoc.Default.Register<AgesInfoDatasVM>();
            SimpleIoc.Default.Register<ServerInfoDatasVM>();
            SimpleIoc.Default.Register<CameraInfoDatasVM>();
            SimpleIoc.Default.Register<UserInfoDatasVM>();
            SimpleIoc.Default.Register<NewRoomVM>();
            SimpleIoc.Default.Register<NewAgedVM>();
            SimpleIoc.Default.Register<NewServerVM>();
            SimpleIoc.Default.Register<NewCameraVM>();
            SimpleIoc.Default.Register<NewUserVM>();
        }

        public static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.CreateMap<PoseInfo, PoseInfoVm>();
                cfg.CreateMap<RoomInfo, RoomInfoVM>();
                cfg.CreateMap<RoomInfoVM, RoomInfo>();
                cfg.CreateMap<AgesInfo, AgesInfoVM>();
                cfg.CreateMap<AgesInfoVM, AgesInfo>();
                cfg.CreateMap<ServerInfo, ServerInfoVM>();
                cfg.CreateMap<ServerInfoVM, ServerInfo>();
                cfg.CreateMap<CameraInfo, CameraInfoVM>();
                cfg.CreateMap<CameraInfoVM, CameraInfo>();
                cfg.CreateMap<UserInfo, UserInfoVM>();
                cfg.CreateMap<UserInfoVM, UserInfo>();
            });

            return config;
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ChartViewVM ChartViewVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChartViewVM>();
            }
        }

        public DataManagerVM DataManagerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DataManagerVM>();
            }
        }

        public RoomInfoDatasVM RoomInfoDatasVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RoomInfoDatasVM>();
            }
        }

        public AgesInfoDatasVM AgesInfoDatasVM
        {
            get => ServiceLocator.Current.GetInstance<AgesInfoDatasVM>();
        }

        public ServerInfoDatasVM ServerInfoDatasVM
        {
            get => ServiceLocator.Current.GetInstance<ServerInfoDatasVM>();
        }

        public CameraInfoDatasVM CameraInfoDatasVM
        {
            get => ServiceLocator.Current.GetInstance<CameraInfoDatasVM>();
        }

        public UserInfoDatasVM UserInfoDatasVM
        {
            get => ServiceLocator.Current.GetInstance<UserInfoDatasVM>();
        }

        public HelpVM HelpVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HelpVM>();
            }
        }

        public NewRoomVM NewRoomVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewRoomVM>(Guid.NewGuid().ToString());//每次都重新生成一个新的实例
            }
        }

        public NewAgedVM NewAgedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewAgedVM>(Guid.NewGuid().ToString());
            }
        }

        public NewServerVM NewServerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewServerVM>(Guid.NewGuid().ToString());
            }
        }

        public NewCameraVM NewCameraVM
        {
            get => ServiceLocator.Current.GetInstance<NewCameraVM>(Guid.NewGuid().ToString());
        }

        public NewUserVM NewUserVM
        {
            get => ServiceLocator.Current.GetInstance<NewUserVM>(Guid.NewGuid().ToString());
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}