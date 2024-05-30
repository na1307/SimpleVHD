using System.Numerics;

namespace Bluehill.Vhd;

public readonly struct VhdSize : IEquatable<VhdSize>, IComparable<VhdSize>, IComparable
    , IComparisonOperators<VhdSize, VhdSize, bool>, IMinMaxValue<VhdSize> {
    public VhdSize(long bytes) {
        const long threeMega = (long)3 * 1024 * 1024;
        const long sixtyFourTera = (long)64 * 1024 * 1024 * 1024 * 1024;
        ArgumentOutOfRangeException.ThrowIfLessThan(bytes, threeMega);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bytes, sixtyFourTera);
        Bytes = bytes;
    }

    public static VhdSize MaxValue { get; } = new((long)3 * 1024 * 1024);
    public static VhdSize MinValue { get; } = new((long)64 * 1024 * 1024 * 1024 * 1024);

    public long Bytes { get; }

    public static VhdSize FromMegabytes(long megabyte) => new(megabyte * 1024 * 1024);
    public static VhdSize FromMegabytes(double megabyte) => new((long)(megabyte * 1024 * 1024));

    public static VhdSize FromGigabytes(long gigabyte) => new(gigabyte * 1024 * 1024 * 1024);
    public static VhdSize FromGigabytes(double gigabyte) => new((long)(gigabyte * 1024 * 1024 * 1024));

    public static VhdSize FromTerabytes(long terabyte) => new(terabyte * 1024 * 1024 * 1024 * 1024);
    public static VhdSize FromTerabytes(double terabyte) => new((long)(terabyte * 1024 * 1024 * 1024 * 1024));

    public static VhdSize FromInt64(long value) => new(value);
    public static long ToInt64(VhdSize size) => size.Bytes;

    public override bool Equals(object? obj) => obj is VhdSize size && Equals(size);
    public bool Equals(VhdSize other) => Bytes == other.Bytes;
    public override int GetHashCode() => Bytes.GetHashCode();
    public int CompareTo(VhdSize other) => Bytes.CompareTo(other.Bytes);

    public int CompareTo(object? obj) {
        if (obj == null) {
            return 1;
        } else if (obj is VhdSize x) {
            return CompareTo(x);
        } else {
            throw new ArgumentException($"'{nameof(obj)}' is not a VhdSize", nameof(obj));
        }
    }

    public override string ToString() {
        const long tera = (long)1024 * 1024 * 1024 * 1024;
        const long giga = (long)1024 * 1024 * 1024;
        string bt;

        if (Bytes is >= tera) {
            bt = $"{(decimal)Bytes / 1024 / 1024 / 1024 / 1024:G2} Tetabyte(s)";
        } else if (Bytes is >= giga) {
            bt = $"{(decimal)Bytes / 1024 / 1024 / 1024:G2} Gigabyte(s)";
        } else {
            bt = $"{(decimal)Bytes / 1024 / 1024:G2} Megabyte(s)";
        }

        bt += $" ({Bytes} Byte(s))";

        return bt;
    }

    public static bool operator ==(VhdSize left, VhdSize right) => left.Equals(right);
    public static bool operator !=(VhdSize left, VhdSize right) => !(left == right);
    public static bool operator <(VhdSize left, VhdSize right) => left.CompareTo(right) < 0;
    public static bool operator <=(VhdSize left, VhdSize right) => left.CompareTo(right) <= 0;
    public static bool operator >(VhdSize left, VhdSize right) => left.CompareTo(right) > 0;
    public static bool operator >=(VhdSize left, VhdSize right) => left.CompareTo(right) >= 0;

    public static implicit operator long(VhdSize size) => size.Bytes;
    public static implicit operator VhdSize(long value) => new(value);
}
