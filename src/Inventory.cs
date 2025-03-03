class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    //methods
    public bool Put(string itemName, Item item)
    {
        if (item.Weight > FreeWeight())
        {
            System.Console.WriteLine("Not enough space");
            return false;
        }
        items.Add(itemName, item);
        return true;
    }

    public Item Get(string itemName)
    {
        if (!items.ContainsKey(itemName))
        {
            System.Console.WriteLine("item couldn't be found.");
            return null;
        }

        Item item = items[itemName];
        items.Remove(itemName);
        return item;
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.Weight;
        }
        return total;
    }

    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }
}