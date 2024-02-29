using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigoController : MonoBehaviour
{
    private Animator animator;
    private Game game;

    // Verifica el componente game y animator
    void Start()
    {
        game = FindObjectOfType<Game>();
        if (game == null)
        {
            Debug.LogError("No se encontró una instancia de Game en la escena.");
            return;
        }

        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        controlClick();
    }
    //Activar animacion del enemigo
    private void controlClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("click", true);
        }
        else
        {
            animator.SetBool("click", false);
        }
    }

}
