/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:AIForAgedClient"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AIForAgedClient.Model;
using AutoMapper;
using CommonServiceLocator;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Net.Http;

namespace AIForAgedClient.ViewModel
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
            SimpleIoc.Default.Register(() => new HttpClient());
            SimpleIoc.Default.Register(() => new Mapper(CreateConfiguration()));
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PoseInfoesVM>();
            SimpleIoc.Default.Register<DataManagerVM>();
            SimpleIoc.Default.Register<ChartViewVM>();
            SimpleIoc.Default.Register<RoomInfoDatasVM>();
            SimpleIoc.Default.Register<AgesInfoDatasVM>();
            SimpleIoc.Default.Register<ServerInfoDatasVM>();
            SimpleIoc.Default.Register<CameraInfoDatasVM>();
            SimpleIoc.Default.Register<NewRoomVM>();
            SimpleIoc.Default.Register<NewAgedVM>();
            SimpleIoc.Default.Register<NewServerVM>();
            SimpleIoc.Default.Register<NewCameraVM>();
            SimpleIoc.Default.Register<MonitorViewModel>();
            SimpleIoc.Default.Register<DetailPoseInfoVM>();
            SimpleIoc.Default.Register<RoomViewModel>();
            SimpleIoc.Default.Register<ManyPersonMonitorVM>();
            //string video_type = ConfigurationManager.AppSettings["video_type"].Trim();
            //if (video_type == "orignal")
            //    SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
            //else if (video_type == "huo_chai_ren")
            //    SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiRenFourVideoVM>();
            //else
            //    SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
        }

        public static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.CreateMap<PoseInfo, PoseInfoVM>();
                cfg.CreateMap<RoomInfo, RoomInfoVM>();
                cfg.CreateMap<RoomInfoVM, RoomInfo>();
                cfg.CreateMap<AgesInfo, AgesInfoVM>();
                cfg.CreateMap<AgesInfoVM, AgesInfo>();
                cfg.CreateMap<ServerInfo, ServerInfoVM>();
                cfg.CreateMap<ServerInfoVM, ServerInfo>();
                cfg.CreateMap<CameraInfo, CameraInfoVM>();
                cfg.CreateMap<CameraInfoVM, CameraInfo>();
                cfg.CreateMap<PaginatedList<DetailPoseInfo>, PaginatedListVM<DetailPoseInfo>>();
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

        public PoseInfoesVM PoseInfoesVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PoseInfoesVM>();
            }
        }

        public RoomViewModel RoomVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RoomViewModel>();
            }
        }

        public ManyPersonMonitorVM ManyPersonMonitorVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ManyPersonMonitorVM>();
            }
        }

        public MonitorViewModel Monitor
        {
            get => ServiceLocator.Current.GetInstance<MonitorViewModel>(Guid.NewGuid().ToString());//每次都重新生成一个新的实例
        }

        public DetailPoseInfoVM DetailPoseInfo
        {
            get => ServiceLocator.Current.GetInstance<DetailPoseInfoVM>(Guid.NewGuid().ToString());//每次都重新生成一个新的实例
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}