using System;
using System.Threading.Tasks;
using static GamePaths;

class Fortnite
{
    public static async Task Launch()
    {
        try
        {
            await RequiredFilesDownloader.Download();
            if (Anticheat.CheckForCorruption() != Anticheat.EPlayStatus.Playable)
                return;

            await EAC.Execute(EACOperation.Initialize);
            await FNProc.Launch(Executables.FortniteLauncher.Process());
            await FNProc.Launch(Executables.FortniteClient_Win64_Shipping_BE.Process());
            await FNProc.Launch(Executables.FortniteClient_Win64_Shipping_EAC.Process());
            await FNProc.Launch(Executables.FortniteClient_Win64_Shipping.Process());

            LaunchStatusService.OnGameOpened();
        }
        catch (Exception Error)
        {
            DialogService.ShowSimpleDialog($"{Error.Message}", "Whoops! Something went wrong.");
        }
    }
}