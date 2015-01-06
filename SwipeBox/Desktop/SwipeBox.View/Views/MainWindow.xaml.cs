using System.Windows;
using SwipeBox.UI.ViewModel;
using Microsoft.Practices.Unity;
namespace SwipeBox.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       [Dependency]
       public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
