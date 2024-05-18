namespace SimpleVhd;

public sealed class Vhd {
    public required string Name { get; set; }
    public required string Directory { get; init; }
    public required string FileName { get; init; }
    public required Style Style { get; set; }
    public required VhdType Type { get; set; }
    public required VhdFormat Format { get; set; }
    public required Guid ParentGuid { get; init; }
}
