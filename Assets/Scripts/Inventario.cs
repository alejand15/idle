using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public GameObject UIinventario;
    public GameObject UIpanelSlots;

    
    private void Start()
    {
        
    }
    public void InventarioScreen() { UIinventario.gameObject.SetActive(true); }
    public void InventarioCerrar() { UIinventario.gameObject.SetActive(false); }


}