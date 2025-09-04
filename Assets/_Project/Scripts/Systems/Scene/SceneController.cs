using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlador central de carregamento de cenas com suporte a tela de loading.
/// Permite carregar cenas de forma síncrona ou assíncrona, além de descarregar cenas.
/// </summary>
public class SceneController : MonoBehaviour
{
    // Singleton para fácil acesso global
    public static SceneController Instance { get; private set; }

    [Header("Loading UI")]
    [Tooltip("Objeto de tela de loading (Canvas ou painel)")]
    public GameObject loadingScreen;

    [Tooltip("Barra de progresso opcional para tela de loading")]
    public Slider progressBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantém entre cenas
    }

    /// <summary>
    /// Carrega uma cena de forma síncrona
    /// </summary>
    /// <param name="scene">Cena a ser carregada</param>
    public void LoadScene(SceneNames scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    /// <summary>
    /// Carrega uma cena de forma assíncrona, mostrando a tela de loading
    /// </summary>
    /// <param name="scene">Cena a ser carregada</param>
    public void LoadSceneAsync(SceneNames scene)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        StartCoroutine(LoadSceneCoroutine(scene));
    }

    /// <summary>
    /// Coroutine responsável pelo carregamento assíncrono da cena
    /// </summary>
    /// <param name="scene">Cena a ser carregada</param>
    /// <returns>IEnumerator para execução da Coroutine</returns>
    private IEnumerator LoadSceneCoroutine(SceneNames scene)
    {
        // Inicia o carregamento assíncrono da cena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)scene);
        asyncLoad.allowSceneActivation = false; // Evita ativar imediatamente

        while (!asyncLoad.isDone)
        {
            // Atualiza a barra de progresso, se houver
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            // Quando o carregamento atingir 90%, permite ativação da cena
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Desativa a tela de loading após a cena ser carregada
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    /// <summary>
    /// Descarrega uma cena específica
    /// </summary>
    /// <param name="scene">Cena a ser descarregada</param>
    public void UnloadScene(SceneNames scene)
    {
        if (SceneManager.GetSceneByBuildIndex((int)scene).isLoaded)
        {
            SceneManager.UnloadSceneAsync((int)scene);
        }
    }

    /// <summary>
    /// Obtém a cena atualmente ativa
    /// </summary>
    /// <returns>Nome da cena ativa</returns>
    public SceneNames GetActiveScene()
    {
        return (SceneNames)SceneManager.GetActiveScene().buildIndex;
    }
}
