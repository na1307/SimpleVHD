﻿using Bluehill.Bcd;
using System.Text.Json;
using System.Text.Json.Nodes;
using static SimpleVhd.Installer.DevicePathMapper;

namespace SimpleVhd.Installer;

public sealed class NewInstallProcessor : InstallProcessor {
    public override void InstallProcess() {
        var vp = getVhdPath(getSystemDiskNumber());
        VhdDrive = Path.GetPathRoot(vp)!.TrimEnd('\\');
        VhdPath = Path.GetDirectoryName(vp[2..])!;

        if (VhdPath.Length > 1) {
            VhdPath += Path.DirectorySeparatorChar.ToString();
        }

        VhdFileName = Path.GetFileNameWithoutExtension(vp);
        Format = Enum.Parse<VhdFormat>(Path.GetExtension(vp)[1..], true);

        Directory.CreateDirectory(Path.Combine(SVDir, BackupDirName));
#pragma warning disable S125 // Sections of code should not be commented out
        //Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", "SimpleVHD Startup", Path.Combine(SVDir, "Bin", "Startup.exe"), RegistryValueKind.String);
#pragma warning restore S125 // Sections of code should not be commented out

        string arch;

        if (Status.IsX64) {
            arch = "x64";
        } else if (Status.IsArm64) {
            arch = "arm64";
        } else {
            throw new InvalidOperationException();
        }

        var parrent = BcdStore.SystemStore.OpenObject(WellKnownGuids.Current);

        var child1 = BcdStore.SystemStore.CopyObject(parrent, CopyObjectOptions.CreateNewId);
        child1.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child1.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child1Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        var child2 = BcdStore.SystemStore.CopyObject(parrent, CopyObjectOptions.CreateNewId);
        child2.SetVhdDeviceElement(BcdElementType.BcdLibraryApplicationDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);
        child2.SetVhdDeviceElement(BcdElementType.BcdOSLoaderOSDevice, $"{VhdPath}{Child2Name}.{Format.ToString().ToLower()}", DeviceType.PartitionDevice, null, GetDevicePath(VhdDrive), 0);

        var ramdisk = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.Device);
        ramdisk.SetPartitionDeviceElement(BcdElementType.BcdDeviceSdiDevice, DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        ramdisk.SetStringElement(BcdElementType.BcdDeviceSdiPath, SVPath + "Boot\\boot.sdi");

        var pe = BcdStore.SystemStore.CreateObject(Guid.NewGuid(), BcdObjectType.BootLoader);
        pe.SetStringElement(BcdElementType.BcdLibraryDescription, "SimpleVHD PE");
        pe.SetFileDeviceElement(BcdElementType.BcdLibraryApplicationDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdLibraryApplicationPath, $@"\windows\system32\winload.{(Firmware.IsWindowsUEFI ? "efi" : "exe")}");
        pe.SetObjectListElement(BcdElementType.BcdLibraryInheritedObjects, BcdStore.SystemStore.OpenObject(WellKnownGuids.BootLoaderSettings));
        pe.SetFileDeviceElement(BcdElementType.BcdOSLoaderOSDevice, DeviceType.RamdiskDevice, ramdisk, $"{SVPath}Boot\\{arch}.wim", DeviceType.PartitionDevice, null, GetDevicePath(SVDrive));
        pe.SetStringElement(BcdElementType.BcdOSLoaderSystemRoot, "\\windows");
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderDetectKernelAndHal, true);
        pe.SetBooleanElement(BcdElementType.BcdOSLoaderWinPEMode, true);

        var bootmgr = BcdStore.SystemStore.OpenObject(WellKnownGuids.BootMgr);
        bootmgr.SetIntegerElement(BcdElementType.BcdBootMgrTimeout, 5);
        bootmgr.SetObjectListElement(BcdElementType.BcdBootMgrDisplayOrder, [.. ((BcdObject[])bootmgr.GetElement(BcdElementType.BcdBootMgrDisplayOrder)), pe]);

        using FileStream fs = new(Path.Combine(Application.StartupPath, "..", SettingsFileName), FileMode.Create, FileAccess.Write, FileShare.None);
        using Utf8JsonWriter writer = new(fs, new JsonWriterOptions() { Indented = true });

        new JsonObject() {
            { "$schema", SchemaUrl },
            {
                "VhdInstances",
                (JsonArray)([
            new JsonObject() {
                { nameof(Name), Name },
                { "Directory", VhdPath },
                { "ParentFile", VhdFileName },
                { "Style", Style.Normal.ToString() },
                { nameof(Type), Type.ToString() },
                { nameof(Format), Format.ToString() },
                { "ParentGuid", parrent.Id.ToString("B") },
                { "Child1Guid", child1.Id.ToString("B") },
                { "Child2Guid", child2.Id.ToString("B") }
            },
                ])
            },
            { "RamdiskGuid", ramdisk.Id.ToString("B") },
            { "PEGuid", pe.Id.ToString("B") },
        }.WriteTo(writer);
    }
}
