using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneData : MonoBehaviour
{
    private string CurrentScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    private string SceneSavePath =>
        Path.Combine(Application.persistentDataPath, "Saves", $"{CurrentScene}.json");
    private List<Collectables> collectables = new();
    private List<CheckPoint> checkPoints = new();

    // -------------------------
    // VISUALIZAÇÃO NO INSPECTOR
    // -------------------------
    [Header("Debug - Dados carregados do Save")]
    [SerializeField] private SceneSaveData loadedData;

    private void Awake()
    {
        Debug.Log($"<color=yellow>[SceneData]</color> Scan da cena e carregamento…");
        ScanSceneForSaveables();
        if (!File.Exists(SceneSavePath))
        {
            CreateDefaultSceneSave();
        }
        LoadSceneData();
    }

    // -------------------------------------------------------
    // SCAN DA CENA
    // -------------------------------------------------------
    private void ScanSceneForSaveables()
    {
        collectables.Clear();
        checkPoints.Clear();

        collectables.AddRange(
            FindObjectsByType<Collectables>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        );

        checkPoints.AddRange(
            FindObjectsByType<CheckPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        );

        Debug.Log($"<color=cyan>[SceneData]</color> Encontrados " +
            $"{collectables.Count} coletáveis e {checkPoints.Count} checkpoints na cena.");
    }

    // -------------------------------------------------------
    // SAVE
    // -------------------------------------------------------
    public void SaveSceneData()
    {
        Debug.Log($"<color=cyan>[SceneData]</color> Salvando dados da cena: <b>{CurrentScene}</b>");

        SceneSaveData sceneSaveData = new SceneSaveData();
        sceneSaveData.sceneName = CurrentScene;

        // ------------------ COLLECTABLES ------------------
        List<CollectableSaveData> collectablesData = new();
        foreach (var c in collectables)
        {
            collectablesData.Add(c.collectableSaveData);
            Debug.Log($"Salvando Collectable {c.collectableSaveData.collectibleID} | Coletado: {c.collectableSaveData.isCollected}");
        }
        sceneSaveData.collectablesData = collectablesData.ToArray();

        // ------------------ CHECKPOINTS ------------------
        List<CheckPointSaveData> checkPointsData = new();
        foreach (var cp in checkPoints)
        {
            checkPointsData.Add(cp.checkPointSaveData);
            Debug.Log($"Salvando CheckPoint {cp.checkPointSaveData.checkPointID} | Ativado: {cp.checkPointSaveData.isActivated}");
        }
        sceneSaveData.checkPointsData = checkPointsData.ToArray();

        Directory.CreateDirectory(Path.GetDirectoryName(SceneSavePath));

        string json = JsonUtility.ToJson(sceneSaveData, true);
        File.WriteAllText(SceneSavePath, json);

        //Atualiza o Inspector!
        loadedData = sceneSaveData;

        Debug.Log($"<color=green>[SceneData]</color> SAVE COMPLETO! ({SceneSavePath})");
    }
    
    // -------------------------------------------------------
    // LOAD
    // -------------------------------------------------------
    public void LoadSceneData()
    {
        Debug.Log($"<color=cyan>[SceneData]</color> Carregando dados da cena: <b>{CurrentScene}</b>");

        if (!File.Exists(SceneSavePath))
        {
            Debug.LogWarning($"Nenhum arquivo de save encontrado para {CurrentScene}");
            return;
        }

        string json = File.ReadAllText(SceneSavePath);
        loadedData = JsonUtility.FromJson<SceneSaveData>(json); // <<< MOSTRA NO INSPECTOR

        // ------------------ PLAYER ------------------
        // if (loadedData.playerData != null)
        // {
        //     GameObject player = GameObject.FindWithTag("Player");
        //     if (player != null)
        //     {
        //         player.transform.position = loadedData.playerData.playerPosition;
        //         Debug.Log($"Player carregado em {player.transform.position}");
        //     }
        // }

        // ------------------ COLLECTABLES ------------------
        foreach (var saved in loadedData.collectablesData)
        {
            var c = collectables.Find(x => x.collectableSaveData.collectibleID == saved.collectibleID);

            if (c == null)
            {
                Debug.LogWarning($"Collectable {saved.collectibleID} não encontrado na cena.");
                continue;
            }

            c.collectableSaveData.isCollected = saved.isCollected;
            Debug.Log($"Restaurado Collectable {saved.collectibleID} = {saved.isCollected}");

            c.LoadData();
        }

        // ------------------ CHECKPOINTS ------------------
        foreach (var saved in loadedData.checkPointsData)
        {
            var cp = checkPoints.Find(x => x.checkPointSaveData.checkPointID == saved.checkPointID);

            if (cp == null)
            {
                Debug.LogWarning($"Checkpoint {saved.checkPointID} não encontrado.");
                continue;
            }

            cp.checkPointSaveData.isActivated = saved.isActivated;
            Debug.Log($"Restaurado CheckPoint {saved.checkPointID} = {saved.isActivated}");
        }

        Debug.Log("<color=green>[SceneData]</color> LOAD COMPLETO!");
    }

    // -------------------------------------------------------
    // RESET
    // -------------------------------------------------------
    public void ResetSceneSave()
    {
        if (File.Exists(SceneSavePath))
            File.Delete(SceneSavePath);

        Debug.Log($"<color=red>[SceneData]</color> Save da cena {CurrentScene} RESETADO!");
    }

    private void CreateDefaultSceneSave()
    {
        Debug.Log($"<color=orange>[SceneData]</color> Criando save padrão para a cena {CurrentScene}...");

        SceneSaveData defaultData = new SceneSaveData();
        defaultData.sceneName = CurrentScene;

        // PLAYER
        // GameObject player = GameObject.FindWithTag("Player");
        // defaultData.playerData = new PlayerSaveData();
        // if (player != null)
        //     defaultData.playerData.playerPosition = player.transform.position;

        // COLLECTABLES
        List<CollectableSaveData> collectablesData = new();
        foreach (var c in collectables)
        {
            collectablesData.Add(new CollectableSaveData
            {
                collectibleID = c.collectableSaveData.collectibleID,
                isCollected = false // padrão
            });
        }
        defaultData.collectablesData = collectablesData.ToArray();

        // CHECKPOINTS
        List<CheckPointSaveData> checkPointsData = new();
        foreach (var cp in checkPoints)
        {
            checkPointsData.Add(new CheckPointSaveData
            {
                checkPointID = cp.checkPointSaveData.checkPointID,
                isActivated = false // padrão
            });
        }
        defaultData.checkPointsData = checkPointsData.ToArray();

        // GARANTIR DIRETÓRIO
        Directory.CreateDirectory(Path.GetDirectoryName(SceneSavePath));

        // SALVAR
        string json = JsonUtility.ToJson(defaultData, true);
        File.WriteAllText(SceneSavePath, json);

        Debug.Log($"<color=green>[SceneData]</color> Save padrão criado em {SceneSavePath}");

        // Atualizar no Inspector
        loadedData = defaultData;
    }

}