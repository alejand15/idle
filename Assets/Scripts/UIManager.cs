using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject resetPassUI;
    public GameObject CajaPopup;
    public TMP_Text popupText;
    public GameObject accManagerCanvas;
    public GameObject gameScreen;



    
    private void Awake()
    {
        //Verifica si la instancia es nula
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            // Si ya existe una instancia, muestra un mensaje de advertencia y la destruye
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Funciones para activar y desactivar pantallas
    public void LoginScreen() 
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        resetPassUI.SetActive(false);
    }
    public void RegisterScreen() 
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        resetPassUI.SetActive(false);
    }
    public void MainGameScreen()
    {
        accManagerCanvas.SetActive(false);
        gameScreen.SetActive(true);
    }
    public void resetPasswordUI()
    {
        resetPassUI.SetActive(true );
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }
    public void ShowPopup(string message)
    {
        popupText.text = message;
        CajaPopup.gameObject.SetActive(true);
        
    }
    public void ClosePopup()
    {
        CajaPopup.gameObject.SetActive(false);
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupText.gameObject.SetActive(false);
    }
}