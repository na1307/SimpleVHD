namespace Bluehill.Vhd;

public readonly struct VhdSize : IEquatable<VhdSize>, IComparable<VhdSize>, IComparable {
    public VhdSize(long bytes) {
        const long threeMega = (long)3 * 1024 * 1024;
        const long sixtyFourTera = (long)64 * 1024 * 1024 * 1024 * 1024;
        ArgumentOutOfRangeException.ThrowIfLessThan(bytes, threeMega);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bytes, sixtyFourTera);
        Value = bytes;
    }

    public long Value { get; }

    public static VhdSize FromMegabyte(long megabyte) => new(megabyte * 1024 * 1024);
    public static VhdSize FromMegabyte(double megabyte) => new((long)(megabyte * 1024 * 1024));

    public static VhdSize FromGigabyte(long gigabyte) => new(gigabyte * 1024 * 1024 * 1024);
    public static VhdSize FromGigabyte(double gigabyte) => new((long)(gigabyte * 1024 * 1024 * 1024));

    public static VhdSize FromTerabyte(long terabyte) => new(terabyte * 1024 * 1024 * 1024 * 1024);
    public static VhdSize FromTerabyte(double terabyte) => new((long)(terabyte * 1024 * 1024 * 1024 * 1024));

    public static VhdSize FromInt64(long value) => new(value);
    public static long ToInt64(VhdSize size) => size.Value;

    public override bool Equals(object? obj) => obj is VhdSize size && Equals(size);
    public bool Equals(VhdSize other) => Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public int CompareTo(VhdSize other) => Value.CompareTo(other.Value);

    public int CompareTo(object? obj) {
        if (obj == null) {
            return 1;
        } else if (obj is VhdSize x) {
            return CompareTo(x);
        } else {
            throw new ArgumentException($"'{nameof(obj)}' is not VhdSize", nameof(obj));
        }
    }

    public override string? ToString() {
        const long tera = (long)1024 * 1024 * 1024 * 1024;
        const long giga = (long)1024 * 1024 * 1024;
        string bt;

        if (Value is >= tera) {
            bt = $"{(decimal)Value / 1024 / 1024 / 1024 / 1024:G2} Tetabytes";
        } else if (Value is >= giga) {
            bt = $"{(decimal)Value / 1024 / 1024 / 1024:G2} Gigabytes";
        } else {
            bt = $"{(decimal)Value / 1024 / 1024:G2} Megabytes";
        }

        bt += $" ({Value} Bytes)";

        return bt;
    }

    public static bool operator ==(VhdSize left, VhdSize right) => left.Equals(right);
    public static bool operator !=(VhdSize left, VhdSize right) => !(left == right);
    public static bool operator <(VhdSize left, VhdSize right) => left.CompareTo(right) < 0;
    public static bool operator <=(VhdSize left, VhdSize right) => left.CompareTo(right) <= 0;
    public static bool operator >(VhdSize left, VhdSize right) => left.CompareTo(right) > 0;
    public static bool operator >=(VhdSize left, VhdSize right) => left.CompareTo(right) >= 0;

    public static implicit operator long(VhdSize size) => size.Value;
    public static implicit operator VhdSize(long value) => new(value);
}
