using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendClient.ViewModel
{
    public class NewUserVM : NewModelVMBase<UserInfoVM>
    {
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public NewUserVM(HttpClient httpClient, Mapper mapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = mapper;
            if (SimpleIoc.Default.IsRegistered<UserInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<UserInfoVM>();
                Title = "修改用户信息";
            }
            else
            {
                isNew = true;
                Model = new UserInfoVM();
                Title = "创建新用户";
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
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetUserInfoUrl"], autoMapper.Map<UserInfo>(Model));
            }
            else
            {
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetUserInfoUrl"], Model.Id, autoMapper.Map<UserInfo>(Model));
            }
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewUserWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<UserInfoVM>();
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewUserWindowLoaded()");
        }
    }
}
