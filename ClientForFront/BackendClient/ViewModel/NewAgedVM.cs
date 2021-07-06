using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendClient.ViewModel
{
    public class NewAgedVM : NewModelVMBase<AgesInfoVM>
    {
        //当为true时是新建对象，为false时是修改对象
        private readonly bool isNew = true;

        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        public ObservableCollection<RoomInfoVM> Rooms { get; } = new ObservableCollection<RoomInfoVM>();

        public RoomInfoVM SelectedRoom { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler _handler = PropertyChanged;
            _handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Year
        {
            get
            {
                return Model.BirthDay.Year;
            }
            set
            {
                int month = Model.BirthDay.Month;
                int day = Model.BirthDay.Day;
                Model.BirthDay = DateTime.Parse(value + "/" + month + "/" + day);
            }
        }

        public int Month
        {
            get
            {
                return Model.BirthDay.Month;
            }
            set
            {
                int year = Model.BirthDay.Year;
                int day = Model.BirthDay.Day;
                Model.BirthDay = DateTime.Parse(year + "/" + value + "/" + day);
            }
        }

        public int Day
        {
            get
            {
                return Model.BirthDay.Day;
            }
            set
            {
                int year = Model.BirthDay.Year;
                int month = Model.BirthDay.Month;
                Model.BirthDay = DateTime.Parse(year + "/" + month + "/" + value);
            }
        }

        public List<int> Years
        {
            get
            {
                List<int> _years = new List<int>();
                int maxAge = 120;
                int startYear = DateTime.Now.Year - maxAge;
                for (int i = 0; i < maxAge; i++)
                {
                    _years.Add(startYear + i);
                }
                return _years;
            }
        }

        public List<int> Months
        {
            get
            {
                List<int> _months = new List<int>(12);
                for (int i = 1; i < 13; i++)
                {
                    _months.Add(i);
                }
                return _months;
            }
        }

        public List<int> Days
        {
            get
            {
                List<int> _days = new List<int>(31);
                for (int i = 1; i < 32; i++)
                {
                    _days.Add(i);
                }
                return _days;
            }
        }

        public NewAgedVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<AgesInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<AgesInfoVM>();
                Title = "修改老人信息";
                Rooms.Add(Model.RoomInfo);
                SelectedRoom = Model.RoomInfo;
            }
            else
            {
                isNew = true;
                Model = new AgesInfoVM();
                Title = "创建新人员";
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewAgedWindowLoaded()");
            UpdateSourceAsync(Rooms);
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewAgedWindowClosing()");
            if (!isNew)
            {
                SimpleIoc.Default.Unregister<AgesInfoVM>();
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
                Model.RoomInfoId = SelectedRoom.Id;
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetAgedsUrl"], autoMapper.Map<AgesInfo>(Model));
            }
            else
            {
                Model.RoomInfoId = SelectedRoom.Id;
                Model.RoomInfo = SelectedRoom;
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetAgedsUrl"], Model.Id, autoMapper.Map<AgesInfo>(Model));
            }
        }

        private async void UpdateSourceAsync(IList<RoomInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetAgesInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var sourceCollection = JsonConvert.DeserializeObject<List<RoomInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<RoomInfoVM>(item));
                    }
                    else
                    {
                        autoMapper.Map(item, exitInfo);
                    }
                }
                //检查有无删减
                for (int i = targetCollection.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in sourceCollection)
                    {
                        if (targetCollection.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        targetCollection.RemoveAt(i);
                    }
                }
            }
        }
    }
}