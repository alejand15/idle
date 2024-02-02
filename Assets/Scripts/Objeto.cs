using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto:MonoBehaviour
{
    private int damageObject;
    private int buyCoins;
    public Objeto(int dmObj, int buyCns) 
    {
        damageObject = dmObj;
        buyCoins = buyCns;
    }
    public int DamageObject
    {
        get { return damageObject; }
        set { damageObject = value; }
    }

    public int BuyCoins
    {
        get { return buyCoins; }
        set { buyCoins = value; }
    }
}
