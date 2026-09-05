using System;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media.Animation;

namespace FortniteLauncher.Pages
{
    public sealed partial class PlayPage : Page
    {
        public static SettingsCard Launch_Button;
        public static ProgressRing ProgressRing;

        private string DownloadInformation = DownloadService.DownloadProgress;

        private readonly string DisplayUsername = Text.DisplayRandomGreeting();
        private readonly string DisplayDescription = Text.PlayPageDescription;

        private static readonly string LaunchButton_Header = Text.LaunchFortniteText;
        private static readonly string LaunchButton_Description = string.Empty;

        public PlayPage()
        {
            InitializeComponent();
            LoadProfileImage();
            Launch_Button = LaunchButton;
            DownloadService.ProgressChanged += OnDownloadProgressChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs EventArgs)
        {
            base.OnNavigatedTo(EventArgs);
            AnimateBlur();
        }

        private void AnimateBlur()
        {
            var Animation = new Storyboard();
            var ColorAnimation = new ColorAnimation
            {
                From = Windows.UI.Color.FromArgb(178, 0, 0, 0),
                To = Windows.UI.Color.FromArgb(204, 0, 0, 0),
                Duration = TimeSpan.FromMilliseconds(1250),
                EnableDependentAnimation = true,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(ColorAnimation, OverlayBrush);
            Storyboard.SetTargetProperty(ColorAnimation, "Color");
            Animation.Children.Add(ColorAnimation);
            Animation.Begin();
        }

        private async void Launch(object Sender, RoutedEventArgs EventArgs)
        {
            if (!Definitions.BindPlayButton)
                return;

            if (!PathHelper.IsPathValid(GlobalSettings.Options.FortnitePath))
            {
                DialogService.ShowSimpleDialog("You haven't selected a Fortnite installation path yet. Go to the Downloads tab and select your game folder.", "Installation Path Required");
                UI.ShowProgressRing((SettingsCard)Sender, false);
                return;
            }

            ShowDownloadProgress();
            UI.ShowProgressRing((SettingsCard)Sender, true);

            await Processes.ForceCloseFortnite();
            await Fortnite.Launch();

            DownloadInfo.IsOpen = false;
        }

        private void OnDownloadProgressChanged(string DownloadStatus)
        {
            DownloadInformation = DownloadStatus;
            DispatcherQueue.TryEnqueue(() => DownloadInfo.SetValue(TeachingTip.SubtitleProperty, DownloadStatus));
        }

        private async void ShowDownloadProgress()
        {
            DownloadInfo.IsOpen = true;
            while (DownloadInfo.IsOpen)
            {
                DispatcherQueue.TryEnqueue(() => DownloadInfo.Subtitle = DownloadService.DownloadProgress);
                await Task.Delay(5);
            }
            DownloadInfo.IsOpen = false;
        }

        private void LoadProfileImage()
        {
            var URL = GlobalSettings.Options.SkinUrl;
            if (!string.IsNullOrEmpty(URL))
                ProfileImageBrush.ImageSource = new BitmapImage(new Uri(URL, UriKind.Absolute));
        }

        private void OpenUri(string URI) => Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = URI });
        private void Tiktok(object Sender, RoutedEventArgs EventArgs) => OpenUri(ProjectDefinitions.Tiktok);
        private void Donations(object Sender, RoutedEventArgs EventArgs) => OpenUri(ProjectDefinitions.DonationsURL);
    }
}