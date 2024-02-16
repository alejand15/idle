using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int coins;
    public int damage;
    public GameObject click;
    public TMP_Text damageClick;
    public TMP_Text coinsNumber;
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
        coinsNumber.text = coins.ToString();
        damageClick.text = damage.ToString();
    }

    public void Clicks()
    {
        conejo.TakeDamage(damage);
        UpdateHealthBar();
    }

    public void ResetClicks()
    {
        conejo.ResetHealth();
        UpdateHealthBar();
    }
}
