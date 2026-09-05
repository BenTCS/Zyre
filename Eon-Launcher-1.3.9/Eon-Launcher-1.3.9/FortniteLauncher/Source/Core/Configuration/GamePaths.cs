public class GamePaths
{
    public enum Executables
    {
        FortniteLauncher,
        FortniteClient_Win64_Shipping_BE,
        FortniteClient_Win64_Shipping_EAC,
        FortniteClient_Win64_Shipping
    }

    public static string GetExecutableName(Executables Exe, bool bAddGameDirectory = false)
    {
        string Result = "";

        if (Exe == Executables.FortniteLauncher)
            Result = "FortniteLauncher.exe";

        if (Exe == Executables.FortniteClient_Win64_Shipping_BE)
            Result = "FortniteClient-Win64-Shipping_BE.exe";

        if (Exe == Executables.FortniteClient_Win64_Shipping_EAC)
            Result = "FortniteClient-Win64-Shipping_EAC.exe";

        if (Exe == Executables.FortniteClient_Win64_Shipping)
            Result = "FortniteClient-Win64-Shipping.exe";

        if (bAddGameDirectory)
            Result = $"{GlobalSettings.Options.FortnitePath}\\FortniteGame\\Binaries\\Win64\\{Result}";

        return Result;
    }
}

public static class ExecutablesExtensions
{
    public static string Process(this GamePaths.Executables Exe, bool bAddGameDirectory = true) => GamePaths.GetExecutableName(Exe, bAddGameDirectory);

    public static string ProcessName(this GamePaths.Executables Exe) => GamePaths.GetExecutableName(Exe, false).Replace(".exe", "");
}