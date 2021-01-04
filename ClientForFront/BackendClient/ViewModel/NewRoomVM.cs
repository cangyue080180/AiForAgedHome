using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendClient.ViewModel
{
    public class NewRoomVM : NewModelVMBase<RoomInfoVM>
    {
        //当为true时是新建对象，为false时是修改对象
        private readonly bool isNew = true;

        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public NewRoomVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<RoomInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<RoomInfoVM>();
                Title = "修改房间信息";
            }
            else
            {
                isNew = true;
                Model = new RoomInfoVM();
                Title = "创建新房间";
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
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetRoomInfoUrl"], autoMapper.Map<RoomInfo>(Model));
            }
            else
            {
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetRoomInfoUrl"], Model.Id, autoMapper.Map<RoomInfo>(Model));
            }
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewRoomWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<RoomInfoVM>();
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewRoomWindowLoaded()");
        }
    }
}