using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

class Player
{
    // fields
    private int health;
    public Room CurrentRoom { get; set; }
    private Inventory backpack;

    // constructor 
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(10);
    }

    public Inventory Backpack
    {
        get { return backpack; }
    }

    //methods
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health >= 100)
        {
            health = 100;
        }
        Console.WriteLine($"You healed yourself. Your hp is {health}/100");
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public int GetHealth()
    {
        return health;
    }

    public bool TakeFromChest(string itemName)
    {
        Item item = CurrentRoom.Chest.Get(itemName);

        if (item == null)
        {
            Console.WriteLine($"{itemName} is not in the chest.");
            return false;
        }

        if (item.Weight > backpack.FreeWeight())
        {
            Console.WriteLine($"You don't have enough space in your backpack");
            CurrentRoom.Chest.Put(itemName, item);
            return false;
        }

        bool success = backpack.Put(itemName, item);
        if (!success)
        {
            CurrentRoom.Chest.Put(itemName, item);
        }
        return true;
    }

    public bool DropToChest(string itemName)
    {
        Item item = backpack.Get(itemName);
        if (item == null)
        {
            Console.WriteLine($"{itemName} is not in your backpack.");
            return false;
        }

        CurrentRoom.Chest.Put(itemName, item);
        Console.WriteLine($"You put {itemName} in the chest.");
        return true;
    }

    public string GetInventory()
    {
        return backpack.GetItems();
    }
}