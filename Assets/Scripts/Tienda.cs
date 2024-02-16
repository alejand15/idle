using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    public GameObject UItienda;
    private Objeto[] objetos;
    private Game game;
    private Inventario inventario;
    public GameObject[] desbloquearPanel;
    public GameObject[] spritesPanel;
    public TMP_Text[] lvlObj;
    public void Start()
    {
        // Inicialización de objetos
        objetos = new Objeto[]
        {
            new Objeto("Rastrillo", 100, 1000, "xddddd"),
            new Objeto("Pala", 1500, 10000, "xddddd"),
            new Objeto("Azada", 2000, 50000, "xddddd"),
            new Objeto("Machete", 10000, 100000, "xddddd"),
            new Objeto("Tijeras", 18000, 900000, "xddddd"),
            new Objeto("Motosierra", 20000, 1500000, "xddddd"),
            new Objeto("Ballesta", 25000, 2000000, "xddddd"),
            new Objeto("Pistola", 30000, 2500000, "xddddd"),
            new Objeto("Escopeta", 36000, 3000000, "xddddd"),
            new Objeto("Minigun", 50000, 4000000, "xddddd")
        };

        game = FindObjectOfType<Game>();
        if (game == null)
        {
            Debug.LogError("No se encontró una instancia de Game en la escena.");
            return;
        }

        inventario = FindObjectOfType<Inventario>();
        if (inventario == null)
        {
            Debug.LogError("No se encontró una instancia de Inventario en la escena.");
            return;
        }
    }
    private void Update()
    {
        
    }
    private void ComprarObjeto(Objeto objeto)
    {

        if (game.coins >= objeto.BuyCoins)
        {
            game.coins -= objeto.BuyCoins;
            //inventario.AgregarObjeto(objeto.gameObject, objeto.Id, objeto.nombreObjeto, objeto.IconoObjeto);
            objeto.Lvl++;

        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
            
    }
    private void DesbloquarObj(int numero)
    {
        desbloquearPanel[numero].SetActive(false);
        spritesPanel[numero].SetActive(true);
        lvlObj[numero].text = "x "+objetos[numero].Lvl.ToString();
    }
   
    public void ComprarRastrillo() { ComprarObjeto(objetos[0]); DesbloquarObj(0); }
    public void ComprarPala() { ComprarObjeto(objetos[1]); DesbloquarObj(1); }
    public void ComprarAzada() { ComprarObjeto(objetos[2]); DesbloquarObj(2); }
    public void ComprarMachete() { ComprarObjeto(objetos[3]); DesbloquarObj(3); }
    public void ComprarTijeras() { ComprarObjeto(objetos[4]); DesbloquarObj(4); }
    public void ComprarMotosierra() { ComprarObjeto(objetos[5]); DesbloquarObj(5); }
    public void ComprarBallesta() { ComprarObjeto(objetos[6]); DesbloquarObj(6); }
    public void ComprarPistola() { ComprarObjeto(objetos[7]); DesbloquarObj(7); }
    public void ComprarEscopeta() { ComprarObjeto(objetos[8]); DesbloquarObj(8); }
    public void ComprarMinigun() { ComprarObjeto(objetos[9]); DesbloquarObj(9); }
    public void TiendaScreen() { UItienda.gameObject.SetActive(true); }
    public void TiendaCerrar() { UItienda.gameObject.SetActive(false); }

}
