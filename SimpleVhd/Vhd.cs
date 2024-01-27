namespace SimpleVhd;

public sealed record class Vhd {
    public required string Directory { get; init; }
    public required string ParentFile { get; init; }
    public required Style Style { get; set; }
    public required VhdType Type { get; set; }
    public required VhdFormat Format { get; set; }
    public required Guid ParentGuid { get; init; }
    public required Guid Child1Guid { get; init; }
    public required Guid Child2Guid { get; init; }
}
