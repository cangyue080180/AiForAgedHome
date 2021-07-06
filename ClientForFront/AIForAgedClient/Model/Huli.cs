using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIForAgedClient.Model
{
    public class Huli : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsPaiNiao { get; set; }
        public bool IsPaiBian { get; set; }
        public bool IsXiYu { get; set; }
        public bool IsWeiYao { get; set; }
        public string Manager { get; set; }

        private bool _isCommit = true;

        public bool IsCommit
        {
            get
            {
                return _isCommit;
            }

            set
            {
                if (_isCommit != value)
                {
                    _isCommit = value;
                    OnPropertyChanged(nameof(IsCommit));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}