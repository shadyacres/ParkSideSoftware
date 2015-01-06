using System.ComponentModel;

namespace SwipeBox.UI.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected void OnNotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
