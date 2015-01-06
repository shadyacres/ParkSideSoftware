using SwipeBox.BusinessLogic;
using SwipeBox.Shared;
namespace SwipeBox.UI.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        MainWindowViewModel m_parent;

        public MenuViewModel(MainWindowViewModel parent)
        {
            // Set parent and commands
            m_parent = parent;

            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            NavigateToClientsCommand = new RelayCommand(NavigateToClients);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
        }

        public RelayCommand NavigateToHomeCommand { get; private set; }

        private void NavigateToHome(object binding)
        {
            m_parent.CurrentViewModel = new HomeViewModel();
        }

        public RelayCommand NavigateToClientsCommand { get; private set; }

        private void NavigateToClients(object binding)
        {
            m_parent.CurrentViewModel = new ClientsViewModel(new ClientsBL());
        }

        public RelayCommand NavigateToSettingsCommand { get; private set; }

        private void NavigateToSettings(object obj)
        {
            m_parent.CurrentViewModel = new SettingsViewModel();
        }
    }
}