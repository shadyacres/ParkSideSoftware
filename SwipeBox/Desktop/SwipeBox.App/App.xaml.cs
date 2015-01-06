using Microsoft.Practices.Unity;
using Ninject;
using SwipeBox.BusinessLogic;
using SwipeBox.DAL.Repositories;
using SwipeBox.UI;
using SwipeBox.UI.View;
using System.Reflection;
using System.Windows;

namespace SwipeBox.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Closing += ApplicationClosing;
            mainWindow.Show();
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void ConfigureContainer()
        {
             container = new UnityContainer();
            container.RegisterType<IClientsBL, ClientsBL>();
            container.RegisterType<IClientRepository, EFClientRepository>();

        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show("An exception occurred",
                                           "Error",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);
            e.Handled = true;
        }

        private void ApplicationClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Are you sure you want to close the application?",
                                                        "Closing",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
