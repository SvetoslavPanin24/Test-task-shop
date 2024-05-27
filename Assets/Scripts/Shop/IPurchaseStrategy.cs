public interface IPurchaseStrategy
{
    string MethodName { get; }
    bool Purchase(IItem item);
}