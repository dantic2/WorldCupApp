using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using DAL.Storage;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static DAL.Services.AppContext Context {get; } = new DAL.Services.AppContext();
        public static AppSettings Settings { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Settings = Context.Storage.LoadSettings();

            if (Settings == null)
            {

                var dlg = new SettingsWindow();
                var ok = dlg.ShowDialog();

                if (ok != true || dlg.SelectedSettings == null)
                {
                    Shutdown();
                    return;
                }

                Settings = dlg.SelectedSettings;
                Context.Storage.SaveSettings(Settings);
            }

            DAL.Services.LocalizationService.ApplyLanguage(Settings?.Language ?? DAL.Storage.AppLanguage.En);

            var main = new MainWindow();
            MainWindow = main;
            main.Show();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}
