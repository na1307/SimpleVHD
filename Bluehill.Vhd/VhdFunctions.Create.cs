namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    public static SafeVirtualDiskHandle CreateVhd(string path, VhdSize size, bool isFixed = false) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return createVhdCore(path, null, null, size, isFixed);
    }

    public static SafeVirtualDiskHandle CreateChildVhd(string child, string parent) {
        ArgumentException.ThrowIfNullOrWhiteSpace(child);
        ArgumentException.ThrowIfNullOrWhiteSpace(parent);

        return createVhdCore(child, parent, null, default, false);
    }

    public static SafeVirtualDiskHandle CloneVhd(string destination, string source, VhdSize size = default, bool isFixed = false) {
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);
        ArgumentException.ThrowIfNullOrWhiteSpace(source);

        return createVhdCore(destination, null, source, size, isFixed);
    }

    private static SafeVirtualDiskHandle createVhdCore(string path, string? parent, string? source, VhdSize size, bool isFixed) {
        if (parent is not null && source is not null) {
            throw new ArgumentException("Both");
        }

        if (parent is not null &&
            !Path.GetExtension(path).TrimStart('.').Equals(Path.GetExtension(parent).TrimStart('.'), StringComparison.OrdinalIgnoreCase)) {
            throw new ArgumentException("The parent and child file extensions must be the same.");
        }

        var vst = getVst(path);

        CreateVirtualDiskParameters cvdp = new() {
            UniqueId = Guid.Empty,
            MaximumSize = (ulong)(long)size,
            BlockSizeInBytes = 0,
            SectorSizeInBytes = 512,
            ParentPath = parent,
            SourcePath = source
        };

        var result = NativeMethods.CreateVirtualDisk(
            in vst,
            path,
            VirtualDiskAccessMask.Create,
            nint.Zero,
            parent is not null && isFixed ? CreateVirtualDiskOptions.FullPhysicalAllocation : CreateVirtualDiskOptions.None,
            0,
            in cvdp,
            nint.Zero,
            out var handle);

        return result == 0 ? handle : throw new OperationFailedException(Marshal.GetPInvokeErrorMessage((int)result));
    }
}
