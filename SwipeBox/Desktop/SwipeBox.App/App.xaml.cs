// <copyright file="App.xaml.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Entry point for application</summary>

using Microsoft.Practices.Unity;
using SwipeBox.BusinessLogic;
using SwipeBox.DAL.Repositories;
using SwipeBox.UI.View;
using System.Windows;
namespace SwipeBox.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;

        /// <summary>
        /// Handle the onStartup event
        /// </summary>
        /// <param name="e">event args</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Closing += ApplicationClosing;
            mainWindow.Show();
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        /// <summary>
        /// Configure the unity container
        /// </summary>
        private void ConfigureContainer()
        {
            container = new UnityContainer();
            container.RegisterType<IClientsBL, ClientsBL>();
            container.RegisterType<IClientRepository, EFClientRepository>();

        }

        /// <summary>
        /// Handle any unhandled exceptions
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show("An exception occurred. The application will now shut down.",
                                           "Error",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        ///  Handle the closing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
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
