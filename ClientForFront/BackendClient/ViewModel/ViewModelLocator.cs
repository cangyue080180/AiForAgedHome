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

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

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

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DataViewVM>();
            SimpleIoc.Default.Register<HelpVM>();
            SimpleIoc.Default.Register<DataManagerVM>();
            SimpleIoc.Default.Register<RoomInfoVM>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public DataViewVM DataViewVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DataViewVM>();
            }
        }
        public DataManagerVM DataManagerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DataManagerVM>();
            }
        }
        public RoomInfoVM RoomInfoVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RoomInfoVM>();
            }
        }

        public HelpVM HelpVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HelpVM>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}