class Player
{

    private int health;
    public Room CurrentRoom { get; set; }

    // constructor 
    public Player()
    {
        CurrentRoom = null;
        health = 100;
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
}