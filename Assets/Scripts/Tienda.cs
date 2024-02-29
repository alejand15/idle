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
    public Objeto[] objetos;
    private Game game;
    public GameObject[] desbloquearPanel;
    public GameObject[] spritesPanel;
    public TMP_Text[] lvlObj;
    public GameObject UIinventario;
    public TMP_Text[] nombreObjetoTxt;
    public TMP_Text[] descripcionObjetoTxt;
    public TMP_Text[] monedasObjetoTxt;
    public int[] nuevosNiveles;
    public DataServer dataServer;
    public DataToSave dts;
    public TMP_Text avisoMonedas;
    private Coroutine avisoCoroutine;

    public void Start()
    {
        // Inicialización de objetos
        objetos = new Objeto[]
        {
            new Objeto("Lanza", 1, 10, "Aumenta 1 de daño en cada compra."),
            new Objeto("Hacha", 10, 100, "Aumenta 10 de daño en cada compra."),
            new Objeto("Arco", 100, 1000, "Aumenta 100 de daño en cada compra."),
            new Objeto("Pistola", 5000, 3000, "Aumenta 5000 de daño en cada compra."),
            new Objeto("Escopeta", 10000, 5500, "Aumenta 10000 de daño en cada compra."),
            new Objeto("Francotirador", 15000, 7500, "Aumenta 15000 de daño en cada compra."),
            new Objeto("Rifle", 25000, 12000, "Aumenta 25000 de daño en cada compra."),
            new Objeto("Famas", 40000, 18000, "Aumenta 40000 de daño en cada compra."),
            new Objeto("Scar-L", 50000, 22000, "Aumenta 50000 de daño en cada compra."),
            new Objeto("AWP", 70000, 30000, "Aumenta 70000 de daño en cada compra.")
        };
        game = FindObjectOfType<Game>();
        // Asegura que dataServer no sea nulo
        if (dataServer == null)
        {
            Debug.LogError("DataServer no asignado en la Tienda.");
            return;
        }

        // Asegura que dts no sea nulo
        if (dts == null)
        {
            Debug.LogError("DataToSave no asignado en la Tienda.");
            return;
        }

        // Carga los datos desde la base de datos
        dataServer.LoadDataFn();

        Debug.Log("Datos cargados desde la base de datos.");

        // Actualiza la información de los objetos después de cargar los datos  
        sustituirInfo();
        InfoObjetoTexto();
    }

    // Actualiza los textos en la interfaz de usuario con información de objetos
    private void InfoObjetoTexto()
    {
        for (int i = 0; i < objetos.Length; i++)
        {
            nombreObjetoTxt[i].text = objetos[i].nombreObjeto;
            descripcionObjetoTxt[i].text = objetos[i].descripcionObj;
            monedasObjetoTxt[i].text = objetos[i].BuyCoins.ToString();
        }
    }


    // Función para comprar un objeto
    private void ComprarObjeto(Objeto objeto)
    {    
        if (game.coins >= objeto.BuyCoins)
        {
            game.coins -= objeto.BuyCoins;
            objeto.Lvl++;
            Debug.Log("Compra exitosa. Nuevo nivel: " + objeto.Lvl);
            game.damage += objeto.DamageObject * objeto.Lvl;
            DesbloquarObj(Array.IndexOf(objetos, objeto));

            SetNivelesObjetos(GetNivelesObjetos());

            // Después de realizar una compra y actualizar niveles en la base de datos
            dataServer.UpdateNivelesObjetos();
            if (avisoCoroutine != null)
            {
                StopCoroutine(avisoCoroutine);
            }
            avisoCoroutine = StartCoroutine(OcultarAvisoMonedas());
        }
        else
        {
            avisoMonedas.text = "No tienes suficientes monedas.";
            if (avisoCoroutine != null)
            {
                StopCoroutine(avisoCoroutine);
            }
            avisoCoroutine = StartCoroutine(OcultarAvisoMonedas());
        }
    }

    // Desbloquea un objeto en la interfaz de usuario
    private void DesbloquarObj(int i)
    {    
        desbloquearPanel[i].SetActive(false);
        spritesPanel[i].SetActive(true);
        lvlObj[i].text = "x " + objetos[i].Lvl.ToString();
    }

    // Desbloquea objetos en la carga del juego
    public void DesbloquearObjetosCarga()
    {
        for (int i = 0; i < objetos.Length; i++)
        {
            if (objetos[i].Lvl > 0)
            {
                DesbloquarObj(i);
            }
        }
    }

    public void sustituir()
    {
        // Sustituye los niveles de objetos en el objeto DataToSave
        dts.objetosNiveles = nuevosNiveles;
    }

    // Establece los niveles de objetos y actualiza el DataServer
    public void SetNivelesObjetos(int[] nuevosNiveles)
    {
        if (dts != null)
        {
            dts.objetosNiveles = nuevosNiveles;

            // Actualiza el DataServer
            if (dataServer != null)
            {
                dataServer.UpdateNivelesObjetos();
            }
        }
    }

    // Sustituye la información de los objetos
    public void sustituirInfo()
    {
        for (int i = 0; i < objetos.Length; i++)
        {
            objetos[i].Lvl = dts.objetosNiveles[i];
        }
    }

    // Obtiene los niveles de los objetos
    public int[] GetNivelesObjetos()
    {
        int[] niveles = new int[objetos.Length];

        for (int i = 0; i < objetos.Length; i++)
        {
            niveles[i] = objetos[i].Lvl;
        }

        return niveles;
    }
    // Oculta el aviso de monedas
    private IEnumerator OcultarAvisoMonedas()
    {
        yield return new WaitForSeconds(2f);
        avisoMonedas.text = "";
    }
    // Comprar los objetos
    public void ComprarLanza() { ComprarObjeto(objetos[0]); }
    public void ComprarHacha() { ComprarObjeto(objetos[1]); }
    public void ComprarArco() { ComprarObjeto(objetos[2]); }
    public void ComprarPistola() { ComprarObjeto(objetos[3]); }
    public void ComprarEscopeta() { ComprarObjeto(objetos[4]); }
    public void ComprarFrancotirador() { ComprarObjeto(objetos[5]); }
    public void ComprarRifle() { ComprarObjeto(objetos[6]); }
    public void ComprarFamas() { ComprarObjeto(objetos[7]); }
    public void ComprarScar() { ComprarObjeto(objetos[8]); }
    public void ComprarAWP() { ComprarObjeto(objetos[9]); }

    // Pantallas de tienda e inventario
    public void TiendaScreen() { UItienda.gameObject.SetActive(true); }
    public void TiendaCerrar() { UItienda.gameObject.SetActive(false); }
    public void InventarioScreen() { UIinventario.gameObject.SetActive(true); }
    public void InventarioCerrar() { UIinventario.gameObject.SetActive(false); }

}
