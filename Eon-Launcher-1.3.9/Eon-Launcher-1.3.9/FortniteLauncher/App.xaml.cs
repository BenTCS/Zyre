using Microsoft.UI.Xaml;
using System;
using System.Threading;

namespace FortniteLauncher
{
    public partial class App : Application
    {
        private static Mutex? _mutex;
        private Window? _mainWindowInstance;

        public App()
        {
            try
            {
                // InitializeComponent must always run first in WinUI 3 apps
                InitializeComponent();

                // Ensure unique name formatting for the Mutex (avoiding spaces/special chars)
                EnsureSingleInstance();

                // Move heavy or blocking operations out of the UI thread constructor
                Microsoft.UI.Dispatchers.DispatcherQueue.GetForCurrentThread()?.TryEnqueue(() =>
                {
                    try { Processes.ForceCloseFortnite(); } catch { }
                });
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("error.txt", $"Startup Error: {ex.Message}\n{ex.StackTrace}");
            }

            // Global fallback handlers to catch any background/unhandled crashes
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.IO.File.WriteAllText("error.txt", $"Unhandled Exception: {e.ExceptionObject}");
            };
        }

        protected override void OnLaunched(LaunchActivatedEventArgs arguments)
        {
            try
            {
                InitializeMainWindow();
                ConfigureSettings();
            }
            catch (Exception error)
            {
                System.IO.File.WriteAllText("error.txt", $"Launch Error: {error.Message}\n{error.StackTrace}");
            }
        }

        private void EnsureSingleInstance()
        {
            // Clean the project name to ensure it's a valid OS kernel object name
            string mutexName = $"Global\\{ProjectDefinitions.Name?.Replace(" ", "_") ?? "ZyreLauncher"}";
            _mutex = new Mutex(true, mutexName, out bool createdNew);

            if (!createdNew)
            {
                System.IO.File.WriteAllText("error.txt", "Instance Error: Another instance is already running.");
                Environment.Exit(1);
            }
        }

        private void InitializeMainWindow()
        {
            _mainWindowInstance = new MainWindow();
            _mainWindowInstance.Activate();
            GlobalSettings.Windows = _mainWindowInstance;
        }

        private void ConfigureSettings()
        {
            UserSettings.LoadSettings();

            if (GlobalSettings.Options.IsSoundEnabled)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
        }
    }
}