using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Granja
{
    private int animalesTotalGranja;
    private int animalesIgualesTotal;
    private int animalNum;
    private Image spriteAnimal;
    
    public Granja(int animalesIgualesCambio, int animalesCambioGranja, int animalNumTotal)
    { 
        animalesIgualesTotal = animalesIgualesCambio;
        animalesTotalGranja = animalesCambioGranja;
        animalNum = animalNumTotal;
    }
    public void CambioAnimales(int animalesMuertes)
    {
        spriteAnimal = GameObject.Find("Animal").GetComponent<Image>();
        
        if (animalesIgualesTotal == animalesMuertes) 
        {
            spriteAnimal.sprite = Resources.Load<Sprite>("Sprites/Animal" + animalNum);       
            animalNum++;
            animalesMuertes = 0;
        }
        
    }
    public void CambioGranja()
    {
        if(animalesTotalGranja == animalNum) {}
    }
}
