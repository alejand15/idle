using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    Objeto rastrillo;
    Objeto pala;
    Objeto azada;
    Objeto machete;
    Objeto tijeras;
    Objeto motosierra;
    Objeto ballesta;
    Objeto pistola;
    Objeto escopeta;
    Objeto minigun;
    Game game;
    Inventario inventario;
    public Inventario Inventario;
    public void Start()
    {
        rastrillo = new Objeto(100, 1000);
        pala = new Objeto(1500, 10000);
        azada = new Objeto(2000, 50000);
        machete = new Objeto(10000, 100000);
        tijeras = new Objeto(18000, 900000);
        motosierra = new Objeto(20000, 1500000);
        ballesta = new Objeto(25000, 2000000);
        pistola = new Objeto(30000, 2500000);
        escopeta = new Objeto(36000, 3000000);
        minigun = new Objeto(50000, 4000000);
        game = new Game();
        inventario = Inventario;
    }

    private void ComprarObjeto(Objeto objetoPrefab)
    {
        if (game.coins >= objetoPrefab.BuyCoins)
        {
            game.coins -= objetoPrefab.BuyCoins;

            GameObject nuevoObjeto = Instantiate(objetoPrefab.gameObject, Vector3.zero, Quaternion.identity);
            inventario.AgregarObjeto(nuevoObjeto);
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
    }
    public void ComprarRastrillo(){ComprarObjeto(rastrillo);}
    public void ComprarPala() { ComprarObjeto(pala); }
    public void ComprarAzada() { ComprarObjeto(azada); }
    public void ComprarMachete() { ComprarObjeto(machete); }
    public void ComprarTijeras() { ComprarObjeto(tijeras); }
    public void ComprarMotosierra() { ComprarObjeto(motosierra); }
    public void ComprarBallesta() { ComprarObjeto(ballesta); }
    public void ComprarPistola() { ComprarObjeto(pistola); }
    public void ComprarEscopeta() { ComprarObjeto(escopeta); }
    public void ComprarMinigun() { ComprarObjeto(minigun); }
}
