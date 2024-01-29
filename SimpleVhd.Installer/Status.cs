using System.Runtime.InteropServices;

namespace SimpleVhd.Installer;

public static class Status {
    public static InstallProcessor? Processor { get; set; }
    public static bool IsX64 { get; } = RuntimeInformation.ProcessArchitecture == Architecture.X64;
    public static bool IsArm64 { get; } = RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
