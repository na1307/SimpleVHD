namespace SimpleVhd.PE.Operations;

internal static class OperationFactory {
    public static Operation Create(OperationType type) => type switch {
        OperationType.Backup => new Backup(),
        OperationType.Restore => new Restore(),
        _ => throw new ArgumentException("오류!", nameof(type)),
    };
}
