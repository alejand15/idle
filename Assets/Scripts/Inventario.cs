using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    private List<GameObject> objetosEnInventario = new List<GameObject>();

    public void AgregarObjeto(GameObject objeto)
    {
        objetosEnInventario.Add(objeto);
        Debug.Log("Objeto agregado al inventario: " + objeto.name);
    }

    public void MostrarInventario()
    {
        Debug.Log("Objetos en el inventario:");
        foreach (GameObject objeto in objetosEnInventario)
        {
            Debug.Log(objeto.name);
        }
    }
}
