﻿using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class AgesInfoVM : ViewModelBase
    {
        public long Id { get; set; }

        private string _name;
        [StringLength(20)]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _contacterName;
        [StringLength(20)]
        public string ContacterName
        {
            get => _contacterName;
            set => Set(ref _contacterName, value);
        }

        private string _contacterPhone;
        [StringLength(11)]
        public string ContacterPhone
        {
            get => _contacterPhone;
            set => Set(ref _contacterPhone, value);
        }

        private string _nurseName;
        [StringLength(20)]
        public string NurseName
        {
            get => _nurseName;
            set => Set(ref _nurseName, value);
        }

        private string _address;
        [StringLength(100)]
        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        public long RoomInfoId { get; set; }

        private RoomInfoVM roomInfo;
        public RoomInfoVM RoomInfo
        {
            get => roomInfo;
            set => Set(ref roomInfo, value);
        }
    }
}
