using UnityEngine;

public class Enemigo
{
    private int hp;
    private int coinsEnemigo;
    private int maxHealth;
    private GameObject enemigoAnimado;
    private string nombreEnemigo;
    private bool muerteEnemigo = false;

    public Enemigo(string nombreEnemigos, int health, int coinsEnemigos, GameObject enemigo)
    {
        nombreEnemigo = nombreEnemigos;
        hp = health;
        maxHealth = health;
        coinsEnemigo = coinsEnemigos;
        enemigoAnimado = enemigo;
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
        set { maxHealth = value; }
    }

    public int CoinsEnemigo
    {
        get { return coinsEnemigo; }
        set { coinsEnemigo = value; }
    }

    public GameObject EnemigoAnim
    {
        get { return enemigoAnimado; }
    }

    public string NombreEnemigos
    {
        get { return nombreEnemigo; }
        set { nombreEnemigo = value; }
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

    public bool Muerto
    {
        get { return muerteEnemigo; }
        set { muerteEnemigo = value; }
    }
}
