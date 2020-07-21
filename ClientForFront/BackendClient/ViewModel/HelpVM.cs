using GalaSoft.MvvmLight;
using System.Reflection;

namespace BackendClient.ViewModel
{
    public class HelpVM : ViewModelBase
    {
        public string Name
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name.ToString();
            }
        }

        public string Auth
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string Copyright
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            }
        }

        public string Desc
        {
            get
            {
                return "使用咨询：18600510976";
            }
        }
    }
}
