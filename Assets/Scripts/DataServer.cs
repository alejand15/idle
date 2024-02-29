using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

[Serializable]
public class DataToSave
{
    [SerializeField] public string userName;
    [SerializeField] public int totalCoins;
    [SerializeField] public int totalDamage;
    [SerializeField] public int currentLevel;
    [SerializeField] public int[] objetosNiveles;
}

public class DataServer : MonoBehaviour
{
    public DataToSave dts;      
    public string userId;      
    public Game game;          
    public Tienda tienda;       

    DatabaseReference dbRef;    
    private bool userLoggedIn = false; 

    private void Awake()
    {
        // Configuración de Firebase
        FirebaseApp app = FirebaseApp.DefaultInstance;
        if (app == null)
        {
            app = FirebaseApp.Create(new AppOptions
            {
                DatabaseUrl = new Uri("https://idle-game-4cb85-default-rtdb.europe-west1.firebasedatabase.app/")
            });
        }
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Función para guardar datos en Firebase
    public void SaveDataFn()
    {
        if (!userLoggedIn)
        {
            Debug.LogWarning("Intento de guardar datos antes de iniciar sesión.");
            return;
        }

        DatabaseReference userRef = dbRef.Child("users").Child(userId);
        int[] nivelesObjetos = tienda.GetNivelesObjetos();
        Dictionary<string, object> dataMap = new Dictionary<string, object>
        {
            { "totalCoins", game.coins },
            { "totalDamage", game.damage },
            { "currentLevel", game.numNiveles },
            { "userName", dts.userName },
            { "objetosNiveles", nivelesObjetos }
        };

        userRef.UpdateChildrenAsync(dataMap).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Datos guardados correctamente en Firebase.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error al guardar datos en Firebase: " + task.Exception);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    // Función para cargar datos desde Firebase
    public void LoadDataFn()
    {
        DatabaseReference userRef = dbRef.Child("users").Child(userId);
        userRef.GetValueAsync().ContinueWith(task =>
        {
            try
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Comprobar si hay datos
                    if (snapshot != null && snapshot.Exists)
                    {
                        // Actualización de datos del juego desde Firebase
                        game.coins = Convert.ToInt32(snapshot.Child("totalCoins").Value);
                        game.damage = Convert.ToInt32(snapshot.Child("totalDamage").Value);
                        game.numNiveles = Convert.ToInt32(snapshot.Child("currentLevel").Value);

                        // Actualización del nombre de usuario desde Firebase
                        dts.userName = snapshot.Child("userName").Value?.ToString();

                        // Comprobación de existencia de datos para 'objetosNiveles'
                        if (snapshot.Child("objetosNiveles").Exists)
                        {
                            object objetosNivelesValue = snapshot.Child("objetosNiveles").Value;
                            Debug.Log("Datos crudos para 'objetosNiveles': " + objetosNivelesValue);

                            // Manejo de diferentes tipos de datos en 'objetosNiveles'
                            if (objetosNivelesValue is List<object>)
                            {
                                List<object> objetosNivelesList = (List<object>)objetosNivelesValue;
                                int[] objetosNiveles = new int[objetosNivelesList.Count];

                                // Conversión y asignación de datos a 'objetosNiveles'
                                for (int i = 0; i < objetosNivelesList.Count; i++)
                                {
                                    objetosNiveles[i] = Convert.ToInt32(objetosNivelesList[i]);
                                }

                                // Actualización de datos en la Tienda y carga de objetos
                                tienda.nuevosNiveles = objetosNiveles;
                                tienda.sustituir();
                                Debug.Log("Datos cargados correctamente desde Firebase.");
                                tienda.sustituirInfo();
                                tienda.DesbloquearObjetosCarga();
                            }
                            else if (objetosNivelesValue is long[] || objetosNivelesValue is int[])
                            {
                                int[] objetosNiveles = (int[])objetosNivelesValue;
                                tienda.nuevosNiveles = objetosNiveles;
                                tienda.sustituir();
                                Debug.Log("Datos cargados correctamente desde Firebase.");
                            }
                            else
                            {
                                Debug.LogWarning("Los datos para 'objetosNiveles' no son un array de enteros en Firebase.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No se encontraron datos para 'objetosNiveles' en Firebase.");
                        }

                        Debug.Log("Datos cargados correctamente desde Firebase.");
                    }
                    else
                    {
                        Debug.LogWarning("No se encontraron datos para el usuario en Firebase.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al cargar datos desde Firebase: " + ex.Message);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    // Establecer el nombre de usuario
    public void SetUserName(string newUserName)
    {
        dts.userName = newUserName;
        userLoggedIn = true;
    }

    // Actualizar niveles de objetos en Firebase
    public void UpdateNivelesObjetos()
    {
        if (tienda != null)
        {
            int[] nivelesObjetos = tienda.GetNivelesObjetos();
            dts.objetosNiveles = nivelesObjetos;
            tienda.sustituir();
        }
    }
}
