
namespace SwipeBox.Presentation
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const string CurrentViewModelPropertyName = "CurrentViewModel";

        private BaseViewModel m_currentVM;

        public MainWindowViewModel()
        {
            MenuVM = new MenuViewModel(this);
            CurrentViewModel = new HomeViewModel();
            
        }

        public BaseViewModel CurrentViewModel
        {
            get
            {
                return m_currentVM;
            }

            set
            {
                //if (m_currentVM is IAutoRefresher)
                //{
                //    var refresher = m_currentVM as IAutoRefresher;
                //    refresher.Stop();
                //}

                m_currentVM = value;
                OnNotifyPropertyChanged(CurrentViewModelPropertyName);
            }
        }

        public MenuViewModel MenuVM { get; private set; }
    }
}
