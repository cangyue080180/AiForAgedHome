using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendClient.ViewModel
{
    public class NewServerVM : NewModelVMBase<ServerInfoVM>
    {
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public NewServerVM(HttpClient httpClient, Mapper mapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = mapper;
            if (SimpleIoc.Default.IsRegistered<ServerInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<ServerInfoVM>();
                Title = "修改服务器信息";
            }
            else
            {
                isNew = true;
                Model = new ServerInfoVM();
                Title = "创建新服务器";
            }
        }
        public override bool IsNewModel()
        {
            return isNew;
        }

        public async override Task<bool> Ok()
        {
            if (isNew)
            {
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetServerInfoUrl"], autoMapper.Map<ServerInfo>(Model));
            }
            else
            {
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetServerInfoUrl"], Model.Id, autoMapper.Map<ServerInfo>(Model));
            }
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewServerWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<ServerInfoVM>();
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewServerWindowLoaded()");
        }
    }
}
