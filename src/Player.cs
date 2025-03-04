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
            System.Console.WriteLine($"{item} is not in the chest.");
            return false;
        }

        // if (item.Weight > backpack.FreeWeight())
        // {
        //     System.Console.WriteLine("Not enough space in your backpack");
        //     return false;
        // }

        bool success = backpack.Put(itemName, item);
        if (!success)
        {
            CurrentRoom.Chest.Put(itemName, item);
        }

        System.Console.WriteLine($"You took {item} from the chest.");
        return true;
    }

    public bool DropToChest(string itemName)
    {
        Item item = backpack.Get(itemName);
        if (item == null)
        {
            System.Console.WriteLine($"{itemName} is not in your backpack.");
            return false;
        }

        CurrentRoom.Chest.Put(itemName, item);
        System.Console.WriteLine($"You put {itemName} in the chest.");
        return true;
    }
}