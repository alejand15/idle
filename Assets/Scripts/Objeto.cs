using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto
{
    private int lvl;
    private int damageObject;
    private int buyCoins;
    private string nombreObj;
    private string descObj;
     public Objeto(string nObj, int dmObj, int buyCns, string descObj)
     {
        damageObject = dmObj;
        buyCoins = buyCns;
        nombreObj = nObj;
        this.descObj = descObj;
     }
    public int Lvl
    {
        get { return lvl; }
        set { lvl = value; }
    }

    public int DamageObject
    {
        get { return damageObject; }
        set { damageObject = value; }
    }
    public string nombreObjeto
    {
        get { return nombreObj; }
    }
    public int BuyCoins
    {
        get { return buyCoins; }
        set { buyCoins = value; }
    }
    public string descripcionObj
    {
        get { return descObj; }
    }
}
