public class Animal
{
    private int hp;
    private int coinsAnimal;
    private int maxHealth;

    public Animal(int health, int coinsAnimals)
    {
        hp = health;
        maxHealth = health;
        coinsAnimal = coinsAnimals;
    }

    public int Health
    {
        get { return hp; }
        set
        {
            hp = value;

            if (hp > maxHealth)
            {
                hp = maxHealth;
            }
        }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int CoinsAnimal
    {
        get { return coinsAnimal; }
        set { coinsAnimal = value; }
    }

    public void MuerteAnimal(int damage)
    {
        if (hp <= damage)
        {
            if (hp > 0)
            {
                coinsAnimal++;
            }

            hp = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
    }
    public void ResetHealth()
    {
        hp = maxHealth;
    }
}
