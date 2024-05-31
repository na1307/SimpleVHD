using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleVhd.ControlPanel;

#pragma warning disable
public static class RestartHelper {
    private const uint TOKEN_QUERY = 0x0008;
    private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
    private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
    private const uint SE_PRIVILEGE_ENABLED = 0x0002;

    public static void Restart() {
        if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out var th)) {
            throw new Exception("opt failed " + Marshal.GetLastWin32Error().ToString("X"));
        }

        if (!LookupPrivilegeValueW(null, SE_SHUTDOWN_NAME, out var test)) {
            throw new Exception("lpv failed " + Marshal.GetLastWin32Error().ToString("X"));
        }

        TOKEN_PRIVILEGES tp = new() { PrivilegeCount = 1, Privileges = [new() { Attributes = SE_PRIVILEGE_ENABLED, Luid = test }] };

        if (!AdjustTokenPrivileges(th, false, in tp, 100, out _, out _)) {
            throw new Exception("atp failed " + Marshal.GetLastWin32Error().ToString("X"));
        }

        if (!InitiateSystemShutdownExW(null, null, 0, false, true, 0x80000000)) {
            throw new Exception("issex failed " + Marshal.GetLastWin32Error().ToString("X"));
        }
    }

    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool OpenProcessToken(nint ProcessHandle, uint DesiredAccess, out nint TokenHandle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool LookupPrivilegeValueW(
        [MarshalAs(UnmanagedType.LPWStr)] string? lpSystemName,
        [MarshalAs(UnmanagedType.LPWStr)] string lpName,
        out LUID lpLuid);

    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AdjustTokenPrivileges(
        nint TokenHandle,
        [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
        in TOKEN_PRIVILEGES NewState,
        uint BufferLength,
        out TOKEN_PRIVILEGES PreviousState,
        out uint ReturnLength);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool InitiateSystemShutdownExW(
        [MarshalAs(UnmanagedType.LPWStr)] string? lpMachineName,
        [MarshalAs(UnmanagedType.LPWStr)] string? lpMessage,
        uint dwTimeout,
        [MarshalAs(UnmanagedType.Bool)] bool bForceAppsClosed,
        [MarshalAs(UnmanagedType.Bool)] bool bRebootAfterShutdown,
        uint dwReason);

    private struct TOKEN_PRIVILEGES {
        public uint PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct LUID_AND_ATTRIBUTES {
        public LUID Luid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct LUID {
        public uint LowPart;
        public int HighPart;
    }
}
