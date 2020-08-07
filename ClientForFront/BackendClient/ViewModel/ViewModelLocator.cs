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
            SimpleIoc.Default.Register(()=>new Mapper(CreateConfiguration()));
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ChartViewVM>();
            SimpleIoc.Default.Register<HelpVM>();
            SimpleIoc.Default.Register<DataManagerVM>();
            SimpleIoc.Default.Register<RoomInfoDatasVM>();
            SimpleIoc.Default.Register<AgesInfoDatasVM>();
            SimpleIoc.Default.Register<NewRoomVM>();
            SimpleIoc.Default.Register<NewAgedVM>();
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


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}