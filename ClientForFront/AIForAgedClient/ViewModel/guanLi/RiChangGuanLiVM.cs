using AIForAgedClient.Model;
using DataModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AIForAgedClient.ViewModel
{
    public class RiChangGuanLiVM
    {
        private readonly HttpClient httpClient;
        private List<Huli> _huLiRecords = new List<Huli>();

        public List<Huli> HuLis
        {
            get
            {
                return _huLiRecords;
            }
        }

        public string[] Managers
        {
            get
            {
                return new string[] { "王护", "李护" };
            }
        }

        private RelayCommand<Hyperlink> _onCommitCmd;

        public RelayCommand<Hyperlink> OnCommitCmd
        {
            get
            {
                if (_onCommitCmd == null)
                    _onCommitCmd = new RelayCommand<Hyperlink>(Commit);
                return _onCommitCmd;
            }
        }

        private RelayCommand<Hyperlink> _onViewCmd;

        public RelayCommand<Hyperlink> OnViewCmd
        {
            get
            {
                if (_onViewCmd == null)
                    _onViewCmd = new RelayCommand<Hyperlink>(ViewTongJi);
                return _onViewCmd;
            }
        }

        private void ViewTongJi(Hyperlink obj)
        {
            var huli = obj.DataContext as Huli;
            var mainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            mainViewModel.ShowChartViewCommand.Execute(huli.Name);//TODO: 这里传Name只是暂时简单实现，实际使用时应传ID。
        }

        private void Commit(Hyperlink btn)
        {
            var huli = btn.DataContext as Huli;
            huli.IsCommit = false;
        }

        public RiChangGuanLiVM(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            _huLiRecords.Add(new Huli()
            {
                Name = "张三",
                Age = 78,
                IsPaiNiao = false,
                IsPaiBian = false,
                IsXiYu = false,
                IsWeiYao = false,
                Manager = "李护",
                IsCommit = true
            });
        }

        //private async void UpdateSourceAsync(IList<AgesInfoVM> targetCollection)
        //{
        //    string url = ConfigurationManager.AppSettings["GetAgedsUrl"];
        //    url += "/GetAgesInfosWithRoomInfo";
        //    string result;
        //    try
        //    {
        //        result = await httpClient.GetStringAsync(url);
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        result = null;
        //    }

        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        var sourceCollection = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
        //        //检查有无新增
        //        foreach (var item in sourceCollection)
        //        {
        //            var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
        //            if (exitInfo == null)
        //            {
        //                targetCollection.Add(autoMapper.Map<AgesInfoVM>(item));
        //            }
        //            else
        //            {
        //                autoMapper.Map(item, exitInfo);
        //            }
        //        }
        //        //检查有无删减
        //        for (int i = targetCollection.Count - 1; i >= 0; i--)
        //        {
        //            bool isExit = false;
        //            foreach (var item in sourceCollection)
        //            {
        //                if (targetCollection.ElementAt(i).Id == item.Id)
        //                {
        //                    isExit = true;
        //                    break;
        //                }
        //            }

        //            if (!isExit)
        //            {
        //                targetCollection.RemoveAt(i);
        //            }
        //        }
        //    }
        //}
    }
}