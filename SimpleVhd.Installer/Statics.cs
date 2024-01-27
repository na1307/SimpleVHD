using System.Runtime.InteropServices;

namespace SimpleVhd.Installer;

public static class Statics {
    public static InstallData? Data { get; set; }
    public static bool IsX64 => RuntimeInformation.ProcessArchitecture == Architecture.X64;
    public static bool IsArm64 => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
