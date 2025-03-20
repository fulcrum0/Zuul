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
            Die();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        if (!IsAlive())
        {
            Console.WriteLine("You died. Game over.");
            Environment.Exit(0);
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
        Backpack.Remove("medkit");
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

    public void AttackKnife()
    {
        if (CurrentRoom.HasBlock())
        {
            Console.WriteLine($"Knife is not strong enough to break the barricade.");
        }
        else
        {
            Console.WriteLine($"There is nothing to use knife on.");
        }
    }

    public void AttackBaseballBat()
    {
        if (CurrentRoom.HasBlock() && CurrentRoom.GetBlockName() == "bookshelf")
        {
            Console.WriteLine($"You broke the barricade with the baseball bat.");
            CurrentRoom.Block(null, false);
            Backpack.Remove("baseballBat");
        }
        else
        {
            Console.WriteLine($"There is nothing to use baseball bat on.");
        }
    }

    public void UseKey()
    {
        if (CurrentRoom.HasBlock() && CurrentRoom.GetBlockName() == "padlock")
        {
            Console.WriteLine($"You unlocked the door with the key.");
            CurrentRoom.Block(null, false);
            Backpack.Remove("key");
        }
        else
        {
            Console.WriteLine($"There is nothing to use key on.");
        }
    }
    public string Use(string itemName)
    {
        if (!GetInventory().Contains(itemName))
        {
            Console.WriteLine($"There is no {itemName} in your inventory!");
        }
        else
        {
            switch (itemName)
            {
                case "medkit":
                    Heal(40);
                    break;
                case "knife":
                    AttackKnife();
                    break;
                case "baseballBat":
                    AttackBaseballBat();
                    break;
                case "key":
                    UseKey();
                    break;
            }
        }
        return $"{itemName} cannot be used.";
    }
}