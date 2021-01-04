using AIForAgedClient.Helper;
using AIForAgedClient.Model;
using AutoMapper;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public class DetailPoseInfoVM : ViewModelBase
    {
        #region variable and property

        private HttpClient httpClient;
        private IMapper mapper;
        public string Title { get; private set; }
        public PoseInfoVM PoseInfoVM { get; private set; }

        public PaginatedListVM<DetailPoseInfo> PaginatedListContext { get; private set; }

        private DateTime _dateSelected;

        public DateTime DateSelected
        {
            get => _dateSelected;
            set => Set(() => DateSelected, ref _dateSelected, value);
        }

        public ICommand OnLoadedCommand
        {
            get
            {
                return new RelayCommand(OnWindowLoaded);
            }
        }

        public ICommand OnClosingCommand
        {
            get
            {
                return new RelayCommand(OnWindowClosing);
            }
        }

        public ICommand OnPreDayCmd
        {
            get
            {
                return new RelayCommand(OnPreDayClick, IsEnablePreDayBtn);
            }
        }

        public ICommand OnNextDayCmd
        {
            get
            {
                return new RelayCommand(OnNextDayClick, IsEnableNextDayBtn);
            }
        }

        public ICommand PrePageCmd
        {
            get
            {
                return new RelayCommand(OnPrePageClick);
            }
        }

        public ICommand NextPageCmd
        {
            get
            {
                return new RelayCommand(OnNextPageClick);
            }
        }

        #endregion variable and property

        public DetailPoseInfoVM(PoseInfoVM poseInfoVM, HttpClient httpClient, Mapper mapper)
        {
            this.httpClient = httpClient;
            PoseInfoVM = poseInfoVM;
            this.mapper = mapper;
            PaginatedListContext = new PaginatedListVM<DetailPoseInfo>();
            PaginatedListContext.Items = null;
            Title = PoseInfoVM.AgesInfo.Name + "的姿态详情记录";
        }

        private async Task GetDetailPosesAsync(long id, string date, int pageIndex, int pageSize)
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
                mapper.Map(datas, PaginatedListContext);
            }
        }

        private bool IsEnablePreDayBtn()
        {
            var currentDay = PoseInfoVM.Date;
            var timeSpan = DateSelected - currentDay;
            if (timeSpan.TotalDays > -4)
                return true;

            return false;
        }

        private void OnPreDayClick()
        {
            DateSelected = DateSelected.AddDays(-1);
            PaginatedListContext.PageIndex = 1;
            GetDetailPosesAsync(PoseInfoVM.AgesInfoId, DateSelected.ToShortDateString(), 1, 20);
        }

        private bool IsEnableNextDayBtn()
        {
            var currentDay = PoseInfoVM.Date;
            var timeSpan = DateSelected - currentDay;
            if (timeSpan.TotalDays < 0)
                return true;

            return false;
        }

        private void OnNextDayClick()
        {
            DateSelected = DateSelected.AddDays(1);
            PaginatedListContext.PageIndex = 1;
            GetDetailPosesAsync(PoseInfoVM.AgesInfoId, DateSelected.ToShortDateString(), 1, 20);
        }

        private void OnPrePageClick()
        {
            PaginatedListContext.PageIndex = PaginatedListContext.PageIndex - 1;
            GetDetailPosesAsync(PoseInfoVM.AgesInfoId, DateSelected.ToShortDateString(), PaginatedListContext.PageIndex, 20);
        }

        private void OnNextPageClick()
        {
            PaginatedListContext.PageIndex = PaginatedListContext.PageIndex + 1;
            GetDetailPosesAsync(PoseInfoVM.AgesInfoId, DateSelected.ToShortDateString(), PaginatedListContext.PageIndex, 20);
        }

        private void OnWindowLoaded()
        {
            LogHelper.Debug(nameof(DetailPoseInfoVM) + " Loaded");
            DateSelected = PoseInfoVM.Date;
            GetDetailPosesAsync(PoseInfoVM.AgesInfoId, DateSelected.ToShortDateString(), 1, 20);
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug(nameof(DetailPoseInfoVM) + " Closing");
            SimpleIoc.Default.Unregister<PoseInfoVM>();
        }
    }
}