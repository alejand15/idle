using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int coins;
    public int damage;
    public GameObject click;
    public Text coinsNumber;
    public Image hpBar;
    Animal conejo;
    private void Start()
    {
        conejo = new Animal(50, 500);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        hpBar.fillAmount = (float)conejo.Health / conejo.MaxHealth;
        Debug.Log("Fill Amount: " + hpBar.fillAmount);
    }


    void Update()
    {
    }

    public void Clicks()
    {
        damage++;
        conejo.TakeDamage(damage);
        UpdateHealthBar();

    }

    public void ResetClicks()
    {
        damage = 0;
        conejo.ResetHealth();
        UpdateHealthBar();
    }
}
