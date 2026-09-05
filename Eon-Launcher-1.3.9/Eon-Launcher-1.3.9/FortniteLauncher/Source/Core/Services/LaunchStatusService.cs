using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FortniteLauncher.Pages;

class LaunchStatusService
{
    private const string CloseIcon = "\uE8BB";
    private const string LaunchIcon = "\uE768";

    public static void OnGameOpened()
    {
        SetButtonState(Text.CloseFortniteText, CloseIcon, IsGameRunning: true);
    }

    public static void OnGameClosed(bool ForceClose = false)
    {
        if (ForceClose)
            Processes.ForceCloseFortnite();

        SetButtonState(Text.LaunchFortniteText, LaunchIcon, IsGameRunning: false);
    }

    private static void SetButtonState(string Header, string Icon, bool IsGameRunning)
    {
        var Button = PlayPage.Launch_Button;
        if (Button == null)
            return;

        Button.Click -= OnCloseButtonClick;
        Button.Header = Header;
        Button.Description = string.Empty;
        Button.Content = null;
        Button.HeaderIcon = new FontIcon { Glyph = Icon };
        Definitions.BindPlayButton = !IsGameRunning;

        if (IsGameRunning)
            Button.Click += OnCloseButtonClick;
    }

    private static void OnCloseButtonClick(object Sender, RoutedEventArgs Event)
    {
        OnGameClosed(ForceClose: true);
    }
}