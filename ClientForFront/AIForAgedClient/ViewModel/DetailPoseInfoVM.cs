using AIForAgedClient.Helper;
using AIForAgedClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public class DetailPoseInfoVM : ViewModelBase
    {
        private HttpClient httpClient;
        public string Title { get; private set; }
        public PoseInfoVM PoseInfoVM { get; private set; }

        public PaginatedListVM<DetailPoseInfo> PaginatedListContext { get; private set; }

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

        public DetailPoseInfoVM(PoseInfoVM poseInfoVM,HttpClient httpClient)
        {
            this.httpClient = httpClient;
            PoseInfoVM = poseInfoVM;
            PaginatedListContext = new PaginatedListVM<DetailPoseInfo>();
            PaginatedListContext.Items = null;
            Title = PoseInfoVM.AgesInfo.Name + "的姿态详情记录";
        }

        private async Task GetPosesAsync(long id, string date, int pageIndex, int pageSize)
        {
            string url = ConfigurationManager.AppSettings["GetDetailPoseInfoUrl"];
            url += "?id=" + id;
            url += "&date=" + date;
            url += "&pageIndex=" + pageIndex;
            url += "&pageSize=" + pageSize;
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetDetailPoseInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var datas = JsonConvert.DeserializeObject<PaginatedList<DetailPoseInfo>>(result);
                PaginatedListContext.HasNextPage = datas.HasNextPage;
                PaginatedListContext.HasPreviousPage = datas.HasPreviousPage;
                PaginatedListContext.Items = datas.Items;
                PaginatedListContext.PageIndex = datas.PageIndex;
                PaginatedListContext.TotalPages = datas.TotalPages;
            }
        }

        private void OnWindowLoaded()
        {
            LogHelper.Debug(nameof(DetailPoseInfoVM) + " Loaded");
            GetPosesAsync(PoseInfoVM.AgesInfoId, PoseInfoVM.Date.ToShortDateString(), 1, 20);
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug(nameof(DetailPoseInfoVM) + " Closing");
            SimpleIoc.Default.Unregister<PoseInfoVM>();
        }
    }
}
