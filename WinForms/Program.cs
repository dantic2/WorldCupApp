using DAL.Interfaces;
using DAL.Repo;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace WinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var ctx = new DAL.Services.AppContext();
            var settings = ctx.Storage.LoadSettings();

            DAL.Services.LocalizationService.ApplyLanguage(settings?.Language ?? DAL.Storage.AppLanguage.En);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
         
            Application.Run(new MainForm());
        }

   
    }
}
