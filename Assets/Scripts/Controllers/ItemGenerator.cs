using UnityEngine;

class Eatable : IItem
{
    public string Id => _id;
    public bool IsEatable => true;

    public Eatable(string id) { _id = id; }

    string _id;
}

class Noneatable : IItem
{
    public string Id => _id;
    public bool IsEatable => false;

    public Noneatable(string id) { _id = id; }

    string _id;
}

class EatableGenerator : IItemCreator
{
    string[] _items = { "Cake", "Grape", "Apricot", "Melon", "Raspberries", "Tomato" };
    public IItem CreateItem()
    {
        return new Eatable(_items[Random.Range(0, _items.Length)]);
    }
}

class NoneatableGenerator : IItemCreator
{
    string[] _items = { "Backpack", "Ball", "Flashlight", "Lighthouse", "Rollers", "Telephone" };
    public IItem CreateItem()
    {
        return new Noneatable(_items[Random.Range(0, _items.Length)]);
    }
}

public class ItemGenerator : IItemCreator
{
    IItemCreator[] _creators = { new EatableGenerator(), new NoneatableGenerator() };
    public IItem CreateItem()
    {
        return _creators[Random.Range(0, _creators.Length)].CreateItem();
    }
}
