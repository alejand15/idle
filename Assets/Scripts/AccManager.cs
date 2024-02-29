using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField usernameField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Restart Password")]
    public TMP_InputField emailRestartPassField;

    [Header("Other")]
    public Button logoutButton;
    private FirebaseApp _app;
    public TMP_Text avisoText;
    public DataServer dataServer;
    public Tienda tienda;
    private DataToSave dts;
    public GameObject perfil;
    public GameObject loginUI;
    public GameObject gameUI;
    public TMP_Text usernameText;


    private void Start()
    {
        // Asignación del componente DataServer
        dataServer = GetComponent<DataServer>();
        if (dataServer == null)
        {
            Debug.LogError("DataServer no encontrado en AccManager.");
            return;
        }

        Debug.Log("DataServer: " + (dataServer != null ? "Asignado" : "No Asignado"));

        // Inicialización de Firebase y llamada a la función AutoSave de manera repetitiva

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _app = Firebase.FirebaseApp.DefaultInstance;
                InitializeFirebase();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
        InvokeRepeating("AutoSave", 0f, 120f);
    }

    // Inicialización de Firebase
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");

        string databaseURL = "https://idle-game-4cb85-default-rtdb.europe-west1.firebasedatabase.app/";

        auth = FirebaseAuth.DefaultInstance;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions
                {
                    DatabaseUrl = new System.Uri(databaseURL)
                });
            }

            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
        });
    }

    // Función awake
    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
               
                InitializeFirebase();

                dataServer = GetComponent<DataServer>();
                if (dataServer == null)
                {
                    Debug.LogError("DataServer no encontrado en el objeto AccManager.");
                }
            }
            else
            {
                Debug.LogError("No se pudieron resolver las dependencias de Firebase: " + dependencyStatus);
            }
        });
    }

    // Botón de inicio de sesión
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    // Botón de registro
    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    // Función para el login
    private IEnumerator Login(string _email, string _password)

    {
        // Inicio de sesión asíncrono con Firebase
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        if (auth == null)
        {
            Debug.LogError("Firebase Auth no está inicializado correctamente.");
            yield break;
        }
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        // Manejo de errores
        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Correo o contraseña incorrectos";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Falta Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Falta Contraseña";
                    break;
                case AuthError.WrongPassword:
                    message = "Contraseña equivocada";
                    break;
                case AuthError.InvalidEmail:
                    message = "Correo invalido";
                    break;
                case AuthError.UserNotFound:
                    message = "La cuenta no existe";
                    break;
            }
            UIManager.instance.ShowPopup(message);
        }
        else
        {
            // Éxito en el inicio de sesión
            User = LoginTask.Result.User;

            if (User != null)
            {
                Debug.LogFormat("Usuario registrado correctamente: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                avisoText.text = "Inicio de sesión exitoso";
                yield return new WaitForSeconds(2f);

                avisoText.text = "";
                if (usernameText != null)
                {
                    usernameText.text = "Nombre de usuario: "+User.DisplayName;
                }
                if (dataServer != null)
                {
                    dataServer.SetUserName(User.DisplayName);
                    dataServer.userId = User.UserId;
                }
                dataServer.LoadDataFn();
                yield return new WaitForSeconds(1);
                UIManager.instance.MainGameScreen();
            }
            else
            {
                Debug.LogError("Usuario es null después de iniciar sesión correctamente.");
            }

        }
    }
    // Función para el registro
    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            UIManager.instance.ShowPopup("Falta nombre de usuario");
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            UIManager.instance.ShowPopup("La contraseña no coincide!");
        }
        else
        {
            // Registro asíncrono con Firebase
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            // Manejo de errores durante el registro
            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Registro fallido!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Falta Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Falta Contraseña";
                        break;
                    case AuthError.WeakPassword:
                        message = "Contraseña débil";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Este correo está en uso";
                        break;
                }

                UIManager.instance.ShowPopup(message);
            }
            else
            {
                // Éxito en el registro
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // Actualización del perfil de usuario
                    System.Threading.Tasks.Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
             
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        UIManager.instance.ShowPopup($"Error en nombre de usuario: {firebaseEx.Message}");
                    }
                    else
                    {
                        // Éxito en la actualización del nombre de usuario
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        avisoText.text = "Registro exitoso";
                        yield return new WaitForSeconds(2f);

                        avisoText.text = ""; 
                        if (dataServer != null)
                        {
                            dataServer.SetUserName(_username);
                        }
                    }
                    // Identificador único al usuario
                    string uniqueUserId = User.UserId;
                    dataServer.userId = uniqueUserId;
                }
            }
        }
    }
    // Función para actualizar el nombre de usuario en Firebase
    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };

        System.Threading.Tasks.Task ProfileTask = User.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
    }
    // Función para actualizar el nombre de usuario en la base de datos de Firebase
    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        System.Threading.Tasks.Task DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }
    // Botón para restablecer la contraseña
    public void ResetPasswordButton()
    {
        StartCoroutine(RecuperarContra(emailRestartPassField.text));
    }
    // Función para el restablecimiento de contraseña
    private IEnumerator RecuperarContra(string _email)
    {
        var auth = FirebaseAuth.DefaultInstance;

        var resetTask = auth.SendPasswordResetEmailAsync(_email);
        yield return new WaitUntil(() => resetTask.IsCompleted);

        if (resetTask.Exception != null)
        {
            Debug.LogError($"Failed to reset password: {resetTask.Exception}");
            UIManager.instance.ShowPopup("Error al restablecer la contraseña.");
        }
        else
        {
            Debug.Log("Password reset email sent successfully.");
            UIManager.instance.ShowPopup("Se ha enviado un correo electrónico para restablecer la contraseña.");
        }
    }
    // Guardar datos
    void SaveData()
    {
        if (dataServer != null)
        {
            dataServer.SaveDataFn();
        }
        else
        {
            Debug.LogError("DataServer es nulo en AccManager. Asegúrate de que está asignado correctamente.");
        }
    }

    // Cargar datos
    public void LoadData()
    {
        if (dataServer != null)
        {
            dataServer.LoadDataFn();
        }
        else
        {
            Debug.LogError("DataServer no asignado en AccManager.");
        }
        tienda.SetNivelesObjetos(dts.objetosNiveles);
    }
    // Guardar datos automáticamente
    public void AutoSave()
    {
        if (dataServer != null)
        {
            dataServer.SaveDataFn();
            Debug.Log("Datos guardados automáticamente.");
        }
        else
        {
            Debug.LogError("DataServer no asignado en AccManager. Asegúrate de que está asignado correctamente.");
        }
    }
    // Botón para cerrar sesión
    public void LogoutButton()
    {
        StartCoroutine(Logout());

    }
    // Función para cerrar sesión
    private IEnumerator Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("Sesión cerrada correctamente.");
            loginUI.SetActive(true);
            perfil.SetActive(false);
            gameUI.SetActive(false);

        }
        else
        {
            Debug.LogError("Firebase Auth no está inicializado correctamente.");
        }

        yield return null;
    }
    // Función para cerrar el perfil
    public void cerrarPerfil()
    {
        perfil.SetActive(false);
    }
    // Función para abrir el perfil
    public void abrirPerfil()
    {
        perfil.SetActive(true);
    }
}