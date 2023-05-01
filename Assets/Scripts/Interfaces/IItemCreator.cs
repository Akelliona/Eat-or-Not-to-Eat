public interface IItem
{
    string Id { get; }
    bool IsEatable { get; }
}

public interface IItemCreator
{
    public IItem CreateItem();
}
