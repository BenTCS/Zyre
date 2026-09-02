using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static GamePaths;

class FNProc
{
    public static async Task<Process> Launch(string GamePath)
    {
        Process Process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = FNLaunchHelper.GetDirectory(LaunchInfoType.FileName, GamePath),
                Arguments = FNLaunchHelper.GetDirectory(LaunchInfoType.Arguments, GamePath),
                UseShellExecute = false,
                CreateNoWindow = false,
                WorkingDirectory = FNLaunchHelper.GetDirectory(LaunchInfoType.WorkingDirectory, GamePath)
            }
        };

        Process.Start();

        if (!GamePath.Contains(Executables.FortniteClient_Win64_Shipping.Process(false)))
        {
            foreach (ProcessThread ProcessThread in Process.Threads)
            {
                var ThreadHandle = OpenThread(THREAD_SUSPEND_RESUME, false, ProcessThread.Id);
                if (ThreadHandle != IntPtr.Zero)
                {
                    SuspendThread(ThreadHandle);
                    CloseHandle(ThreadHandle);
                }
            }
        }

        return Process;
    }

    private const int THREAD_SUSPEND_RESUME = 0x0002;

    [DllImport("kernel32.dll")]
    public static extern int SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);
}