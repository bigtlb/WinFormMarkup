using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicApp
{
    public class DataModel : INotifyPropertyChanged
    {
        private int _clickCount;

        public string ClickMessage => $"Number of clicks {_clickCount}";

        public int ClickCount
        {
            get => _clickCount;
            set
            {
                if (_clickCount != value)
                {
                    _clickCount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ClickMessage));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}