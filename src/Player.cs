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
    void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
        }
    }

    void Heal(int amount)
    {
        health += amount;
        if (health >= 100)
        {
            health = 100;
        }
    }

    bool IsAlive()
    {
        return health > 0;
    }

    public int GetHealth()
    {
        return health;
    }
}