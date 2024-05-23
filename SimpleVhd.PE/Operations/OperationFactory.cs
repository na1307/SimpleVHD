namespace SimpleVhd.PE.Operations;

public static class OperationFactory {
    public static Operation Create(OperationType type) {
        return type switch {
            OperationType.Backup => new Backup(),
            OperationType.Restore => new Restore(),
            _ => throw new ArgumentException("오류!", nameof(type)),
        };
    }
}
