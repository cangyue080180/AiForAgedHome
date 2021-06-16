using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public abstract class NewModelVMBase<T>
    {
        public Visibility IsNew
        {
            get
            {
                if (IsNewModel())
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public string Title { get; set; }
        public T Model { get; set; }

        private RelayCommand _onLoadCmd;

        public ICommand OnLoadCmd
        {
            get
            {
                if (_onLoadCmd == null)
                {
                    _onLoadCmd = new RelayCommand(() =>
                    {
                        OnWindowLoaded();
                    });
                }
                return _onLoadCmd;
            }
        }

        private RelayCommand _onClosingCmd;

        public ICommand OnClosingCmd
        {
            get
            {
                if (_onClosingCmd == null)
                {
                    _onClosingCmd = new RelayCommand(() =>
                    {
                        OnWindowClosing();
                    });
                }
                return _onClosingCmd;
            }
        }

        private RelayCommand<Window> _okCmd;

        public RelayCommand<Window> OkCmd
        {
            get
            {
                if (_okCmd == null)
                {
                    _okCmd = new RelayCommand<Window>(
                       async win =>
                       {
                           bool result = await Ok();
                           if (result)
                           {
                               win.DialogResult = true;
                           }
                           win.Close();
                       });
                }
                return _okCmd;
            }
        }

        private RelayCommand<Window> _cancelCmd;

        public RelayCommand<Window> CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand<Window>(win => { win.Close(); });
                }
                return _cancelCmd;
            }
        }

        public abstract bool IsNewModel();

        public abstract void OnWindowLoaded();

        public abstract void OnWindowClosing();

        public abstract Task<bool> Ok();
    }
}