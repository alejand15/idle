using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Variables públicas para monedas, daño, textos y objetos del juego
    public int coins = 0;
    public int damage = 1;
    public TMP_Text damageClick;
    public TMP_Text coinsNumber;
    public TMP_Text nombreEnemigo;
    public TMP_Text numeroNivel;
    public TMP_Text numeroFaltaEnemigos;
    public TMP_Text hpText;
    public Image hpBar;
    public int indice = 0;
    private Enemigo[] enemigos;
    private Enemigo[] nivel;
    public int numNiveles = 1;
    private float fillDuration = 1.0f;
    public GameObject[] enemigosObj;

    private void Start()
    {
        // Inicialización de niveles y enemigos al comienzo del juego
        nivel = new Enemigo[10];
        enemigos = new Enemigo[]
        {
            // Creación de tipos de enemigos
            new Enemigo("Minotauro", 10, 10, enemigosObj[0]),
            new Enemigo("Cilope", 10, 10, enemigosObj[1]),
            new Enemigo("Executoner", 10, 10, enemigosObj[2]),
            new Enemigo("Esqueleto", 10, 10, enemigosObj[3]),
            new Enemigo("Mago", 10, 10, enemigosObj[4]),
            new Enemigo("Mago malvado", 10, 10, enemigosObj[5]),
            new Enemigo("Tooth walker", 10, 10, enemigosObj[6]),
            new Enemigo("Monstruo de basura", 10, 10, enemigosObj [7]),
            new Enemigo("Ciclope de la muerte", 10, 10, enemigosObj[8]),
            new Enemigo("Caballero", 10, 10, enemigosObj[9]),
            new Enemigo("Ninja", 10, 10, enemigosObj[10]),
            new Enemigo("Caballera", 10, 10, enemigosObj[11]),
            new Enemigo("Cabeza de demonio", 10, 10, enemigosObj[12]),
            new Enemigo("Samurai", 10, 10, enemigosObj[13]),
            new Enemigo("Gusano de fuego", 10, 10, enemigosObj[14]),
            new Enemigo("Heroe samurai", 10, 10, enemigosObj[15]),
            new Enemigo("Rey duende", 10, 10, enemigosObj[16]),
        };
        // Genera el primer nivel
        generarNivel();
        // Inicia la actualización de la barra de salud
        StartCoroutine(UpdateHealthBar());
    }

    // Coroutine para actualizar la barra de salud
    private IEnumerator UpdateHealthBar()
    {
        while (true)
        {
            float startTime = Time.time;
            float endTime = startTime + fillDuration;

            while (Time.time < endTime)
            {
                float normalizedTime = (Time.time - startTime) / fillDuration;
                hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, (float)nivel[indice].Health / nivel[indice].MaxHealth, normalizedTime);
                yield return null;
            }

            hpBar.fillAmount = (float)nivel[indice].Health / nivel[indice].MaxHealth;
            yield return null;
        }
    }

    // Actualizar la interfaz de usuario
    void Update()
    {
        coinsNumber.text = coins.ToString();
        damageClick.text = damage.ToString();
        if (nivel != null && indice >= 0 && indice < nivel.Length && nivel[indice] != null)
        {
            hpText.text = nivel[indice].Health.ToString() + " HP";
            nombreEnemigo.text = nivel[indice].NombreEnemigos.ToString();
        }
        else
        {
            hpText.text = "N/A";
            nombreEnemigo.text = "N/A";
        }
        numeroNivel.text = numNiveles.ToString();
        numeroFaltaEnemigos.text = indice.ToString() + " / 10";
    }

    // Función para manejar clics en el juego
    public void Clicks()
    {
        // Reduce la salud del enemigo actual según el daño del jugador
        nivel[indice].TakeDamage(damage);
        // Actualiza la barra de salud
        UpdateHealthBar();
        // Verifica si el enemigo esta muerto
        MuerteEnemigo();
    }
    // Función para determinar la muerte de un enemigo
    public void MuerteEnemigo()
    {
        if (indice < nivel.Length && nivel[indice].Health <= 0 && !nivel[indice].Muerto)
        {
            // Recompensa al jugador y marca al enemigo como muerto
            coins += nivel[indice].CoinsEnemigo;
            nivel[indice].Muerto = true;
            nivel[indice].EnemigoAnim.SetActive(false);
            indice++;

            // Si se han derrotado todos los enemigos genera un nuevo nivel
            if (indice >= nivel.Length)
            {
                generarNivel();
                indice = 0;
            }

            nivel[indice].EnemigoAnim.SetActive(true);
        }
    }

    // Función para generar un nuevo nivel con enemigos aleatorios
    public void generarNivel()
    {
        // Lista de enemigos disponibles
        List<Enemigo> availableEnemigos = new List<Enemigo>(enemigos);
        numNiveles++;
        // Genera enemigos aleatorios para el nuevo nivel
        for (int i = 0; i < nivel.Length; i++)
        {
            if (availableEnemigos.Count == 0)
            {
                Debug.LogError("No hay más enemigos únicos disponibles.");
                break;
            }
            int numRandom = Random.Range(10, 51);
            int index = UnityEngine.Random.Range(0, availableEnemigos.Count);
            nivel[i] = availableEnemigos[index];

            // Ajusta la salud y recompensas en función del nivel
            if (numNiveles >= 3)
            {
                int numHPRandom = Random.Range(10, 51);
                int numCoinsRandom = Random.Range(10, 20);
                nivel[i].MaxHealth = numHPRandom * (numNiveles / 2);
                nivel[i].CoinsEnemigo = numCoinsRandom * numNiveles;
            }
            availableEnemigos.RemoveAt(index);
        }

        // Reinicia la salud y estado de muerte de los enemigos
        for (int i = 0; i < nivel.Length; i++)
        {
            nivel[i].ResetHealth();
            nivel[i].Muerto = false;
        }
        // Activa al primer enemigo del nuevo nivel
        nivel[0].EnemigoAnim.SetActive(true);
        indice = 0;
    }
}
