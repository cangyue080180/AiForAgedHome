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

using AutoMapper;
using CommonServiceLocator;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Configuration;
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
            SimpleIoc.Default.Register(()=>new Mapper(CreateConfiguration()));
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MonitorViewModel>();
            string video_type = ConfigurationManager.AppSettings["video_type"].Trim();
            if (video_type == "orignal")
                SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
            else if (video_type == "huo_chai_ren")
                SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiRenFourVideoVM>();
            else
                SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
        }

        public static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.CreateMap<PoseInfo, PoseInfoVM>();
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

        public MonitorViewModel Monitor
        {
            get => ServiceLocator.Current.GetInstance<MonitorViewModel>(Guid.NewGuid().ToString());//每次都重新生成一个新的实例
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}