using System;

public class Text
{
    // Play Page
    public static string PlayPageDescription = $"Experience the best Chapter {ProjectDefinitions.Chapter} Season {ProjectDefinitions.Season} experience with {ProjectDefinitions.Name}.";
    public static string LaunchFortniteText = "Launch Fortnite";
    public static string CloseFortniteText = "Close Fortnite";

    // Download Page
    public static string DownloadPageTitle = $"Downloading {ProjectDefinitions.Build} Build";
    public static string BuildVersionText = $"{ProjectDefinitions.Name} Build ({ProjectDefinitions.Build}-CL-{ProjectDefinitions.ContentLevel})";
    public static string InstallHeader = $"Install {ProjectDefinitions.Name}";
    public static string InstallBody = $"Download the {ProjectDefinitions.Build} Fortnite Build, essential for playing {ProjectDefinitions.Name}.";
    public static string UninstallHeader = $"Uninstall {ProjectDefinitions.Name}";
    public static string UninstallBody = $"Delete Chapter {ProjectDefinitions.Chapter} Season {ProjectDefinitions.Season} from your computer. This will not uninstall the {ProjectDefinitions.Name} Launcher.";

    public static readonly string[] DownloadMessages = new[]
    {
        "Almost done, please wait",
        "Almost ready to go",
        "Almost there, please wait",
        "Download in progress",
        "Downloading content",
        "Downloading essential files",
        "Downloading required files",
        "Getting things ready for you",
        "Nearly complete, please wait",
        "Nearly there, please wait",
        "Preparing your experience",
        "We're almost there",
        "You're almost ready",
        "You're just moments away",
    };

    public static string DisplayRandomGreeting()
    {
        string Username = GlobalSettings.Options.Username;
        string[] Greetings = new[]
        {
                $"Hello, {Username}!",
                $"Welcome, {Username}!",
                $"Hey, {Username}!",
                $"What's up, {Username}!",
                $"Greetings, {Username}!",
                $"Hi, {Username}!",
                $"Howdy, {Username}!"
        };
        var Random = new Random();
        return Greetings[Random.Next(Greetings.Length)];
    }
}
