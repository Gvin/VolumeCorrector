using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Gvin.Injection;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Presenters;
using VolumeCorrector.Properties;

namespace VolumeCorrector
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            try
            {
                InitializeLocale();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ExceptionLogger.ClearLogs();

                InjectorStorage.Initialize(new InjectorConfiguration());

                using (var volumeMonitor = InjectorStorage.Current.Create<IVolumeMonitor>())
                {
                    if (Settings.Default.Enabled)
                    {
                        volumeMonitor.Start();
                    }

                    using (var presenter = InjectorStorage.Current.Create<INotifyIconPresenter>())
                    {
                        Application.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                ShowErrorMessage();
            }
            finally
            {
                InjectorStorage.Clear();
            }
        }

        private static void InitializeLocale()
        {
            var locale = Settings.Default.LanguageCode;

            if (string.IsNullOrEmpty(locale))
            {
                locale = Thread.CurrentThread.CurrentUICulture.Name.Split('-')[0];
                Settings.Default.LanguageCode = locale;
                Settings.Default.Save();
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
        }

        private static void ShowErrorMessage()
        {
            try
            {
                MessageBox.Show(Resources.Error_Text, Resources.Error_Caption, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                // Suppress exceptions.
            }
        }
    }
}
