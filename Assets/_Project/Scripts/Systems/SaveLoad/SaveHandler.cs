using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System;
public class SaveHandler : MonoBehaviour
{
    [Header("Objetos encontrados na cena (somente leitura)")]
    [SerializeField] private PlayerStartAnimationHandler foundPlayer;
    [SerializeField] private List<Collectables> foundCollectables = new List<Collectables>();
    [SerializeField] private List<CheckPoint> foundCheckpoints = new List<CheckPoint>();
    [SerializeField] private PlayerSpawnpoint playerSpawnpoint;
    [SerializeField] public GameSettingsData gameSettingsData;
    [SerializeField] public SessionProgressionData sessionProgressionData;
    public UIManager uiManager;
    private string sceneName;

    void Awake()
    {
        EventBus.Subscribe<AddCommonCollectableCountEvent>(OnAddCommonCollectable);
        EventBus.Subscribe<AddHiddenCollectableCountEvent>(OnAddHiddenCollectable);
        EventBus.Subscribe<ChangeMasterVolumeEvent>(OnMasterVolumeChanged);
        EventBus.Subscribe<ChangeMusicVolumeEvent>(OnMusicVolumeChanged);
        EventBus.Subscribe<ChangeSFXVolumeEvent>(OnSFXVolumeChanged);
        EventBus.Subscribe<GameLanguageChangeEvent>(OnGameLanguageChanged);
        EventBus.Subscribe<OnSensibilityChange>(OnSensitivityChanged);
    }

    private void OnSensitivityChanged(OnSensibilityChange change)
    {
        gameSettingsData.sensitivity = change.sensibility;
        SaveGameSettings(gameSettingsData);
    }

    private void OnGameLanguageChanged(GameLanguageChangeEvent @event)
    {
        gameSettingsData.gameLanguages = @event.CurrentLanguage;
        SaveGameSettings(gameSettingsData);
    }

    private void OnSFXVolumeChanged(ChangeSFXVolumeEvent @event)
    {
        gameSettingsData.sfxVolume = @event.SFXVolume;
        SaveGameSettings(gameSettingsData);
    }

    private void OnMusicVolumeChanged(ChangeMusicVolumeEvent @event)
    {
        gameSettingsData.musicVolume = @event.MusicVolume;
        SaveGameSettings(gameSettingsData);
    }

    private void OnMasterVolumeChanged(ChangeMasterVolumeEvent @event)
    {
        gameSettingsData.masterVolume = @event.MasterVolume;
        SaveGameSettings(gameSettingsData);
    }

    private void OnAddHiddenCollectable(AddHiddenCollectableCountEvent @event)
    {
        sessionProgressionData.HiddenCollectablesCount++;
    }

    private void OnAddCommonCollectable(AddCommonCollectableCountEvent @event)
    {
        sessionProgressionData.CommonCollectablesCount++;
    }

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        ApplySceneData(LoadOrCreateScene(foundCollectables, foundCheckpoints, playerSpawnpoint));
    }

    //===========================================================
    //  SCANEIA E MOSTRA NO INSPECTOR
    //===========================================================
    [ContextMenu("Atualizar Busca Manualmente")]
    public void ScanSceneObjects()
    {
        foundPlayer = FindAnyObjectByType<PlayerStartAnimationHandler>();
        foundCollectables = new List<Collectables>(FindObjectsByType<Collectables>(FindObjectsSortMode.None));
        foundCheckpoints = new List<CheckPoint>(FindObjectsByType<CheckPoint>(FindObjectsSortMode.None));

        Debug.Log($"Scan concluído — Player {(foundPlayer ? "Localizado" : "Não encontrado")} | " +
                  $"{foundCollectables.Count} coletáveis | {foundCheckpoints.Count} checkpoints");
    }

    //===========================================================
    //                     LOAD
    //===========================================================
    public static SceneSaveData LoadOrCreateScene(List<Collectables> foundCollectables, List<CheckPoint> foundCheckpoints, PlayerSpawnpoint playerSpawnpoint)
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "Scenes/" + SceneManager.GetActiveScene().name + ".json";

        // Garantir diretório
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // Se o arquivo não existir, cria defaults
        if (!File.Exists(path))
        {
            Debug.Log($"<color=yellow>Arquivo de cena não encontrado, criando defaults:</color> {path}");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Criar defaults
            SceneSaveData defaultData = new SceneSaveData
            {
                playerSaveData = new PlayerSaveData { startPosition = playerSpawnpoint.GetSpawnPoint() },
                collectablesSaveData = new List<CollectableSaveData>(),
                checkPointsSaveData = new List<CheckPointSaveData>()
            };

            foreach (var col in foundCollectables)
            {
                defaultData.collectablesSaveData.Add(new CollectableSaveData
                {
                    id = col.name,
                    isCollected = false
                });
            }

            foreach (var cp in foundCheckpoints)
            {
                defaultData.checkPointsSaveData.Add(new CheckPointSaveData
                {
                    id = cp.name,
                    isActivated = false
                });
            }

            File.WriteAllText(path, JsonUtility.ToJson(defaultData, true));
            Debug.Log($"<color=green>Arquivo criado com sucesso:</color> {path}");
            return defaultData;
        }
        // Se já existir, apenas carrega
        string json = File.ReadAllText(path);
        Debug.Log($"<color=blue>Arquivo existente carregado:</color> {path}");
        return JsonUtility.FromJson<SceneSaveData>(json);
    }

    private void ApplySceneData(SceneSaveData data)
    {
        if (data == null)
            return;

        // Player
        if (foundPlayer != null && data.playerSaveData != null && playerSpawnpoint != null)
        {
            foundPlayer.transform.position = data.playerSaveData.startPosition;
            playerSpawnpoint.SetSpawnPoint(data.playerSaveData.startPosition);
        }

        // Collectables
        foreach (var col in foundCollectables)
        {
            var save = data.collectablesSaveData.Find(x => x.id == col.name);
            if (save != null)
            {
                col.collectableSaveData.isCollected = save.isCollected;
                col.LoadData(); // seu método que atualiza a aparência/estado do coletável
            }
        }

        // Checkpoints
        foreach (var cp in foundCheckpoints)
        {
            var save = data.checkPointsSaveData.Find(x => x.id == cp.name);
            if (save != null)
            {
                cp.checkPointSaveData.isActivated = save.isActivated;
            }
        }

        sessionProgressionData = LoadProgression();
        gameSettingsData = LoadGameSettings();
        EventBus.Publish(new ChangeMasterVolumeEvent(gameSettingsData.masterVolume));
        EventBus.Publish(new ChangeMusicVolumeEvent(gameSettingsData.musicVolume));
        EventBus.Publish(new ChangeSFXVolumeEvent(gameSettingsData.sfxVolume));
        EventBus.Publish(new GameLanguageChangeEvent(gameSettingsData.gameLanguages));
        EventBus.Publish(new OnSensibilityChange { sensibility = gameSettingsData.sensitivity });
    }


    //===========================================================
    //                     SAVE
    //===========================================================
    [ContextMenu("Salvar Cena Agora")]
    public void SaveSceneNow()
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "Scenes/" + SceneManager.GetActiveScene().name + ".json";

        // Garantir diretório
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // Se o arquivo não existir, cria defaults
        if (!File.Exists(path))
        {
            Debug.Log($"<color=yellow>Arquivo de cena não encontrado, criando defaults:</color> {path}");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Criar defaults
            SceneSaveData defaultData = new SceneSaveData
            {
                playerSaveData = new PlayerSaveData { startPosition = playerSpawnpoint.GetSpawnPoint() },
                collectablesSaveData = new List<CollectableSaveData>(),
                checkPointsSaveData = new List<CheckPointSaveData>()
            };

            foreach (var col in foundCollectables)
            {
                defaultData.collectablesSaveData.Add(new CollectableSaveData
                {
                    id = col.name,
                    isCollected = false
                });
            }

            foreach (var cp in foundCheckpoints)
            {
                defaultData.checkPointsSaveData.Add(new CheckPointSaveData
                {
                    id = cp.name,
                    isActivated = false
                });
            }

            File.WriteAllText(path, JsonUtility.ToJson(defaultData, true));
            Debug.Log($"<color=green> Arquivo criado com sucesso:</color> {path}");

        }
        else
        {
            // Atualizar dados
            SceneSaveData data = new SceneSaveData
            {
                playerSaveData = new PlayerSaveData
                {
                    startPosition = playerSpawnpoint.GetSpawnPoint()
                },
                collectablesSaveData = new List<CollectableSaveData>(),
                checkPointsSaveData = new List<CheckPointSaveData>()
            };

            foreach (var col in foundCollectables)
            {

                data.collectablesSaveData.Add(new CollectableSaveData
                {
                    id = col.name,
                    isCollected = col.collectableSaveData.isCollected
                });
            }

            foreach (var cp in foundCheckpoints)
            {
                data.checkPointsSaveData.Add(new CheckPointSaveData
                {
                    id = cp.name,
                    isActivated = cp.checkPointSaveData.isActivated
                });
            }
            sessionProgressionData.lastSceneName = SceneManager.GetActiveScene().name;;
            SaveProgression(sessionProgressionData);
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
            Debug.Log($"<color=green> Cena salva com sucesso:</color> {path}");
        }
    }


    //===========================================================
    //                     GAME SETTINGS
    //===========================================================
    public static void SaveGameSettings(GameSettingsData data)
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "GameSettings/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string file = path + "GameSettings.json";
        File.WriteAllText(file, JsonUtility.ToJson(data, true));
        Debug.Log("<color=green> GameSettings salvo com sucesso!</color>");
    }

    public static GameSettingsData LoadGameSettings()
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "GameSettings/GameSettings.json";
        if (!File.Exists(path))
        {
            Debug.Log("<color=yellow>Arquivo GameSettings não encontrado, criando defaults</color>");
            GameSettingsData defaultData = new GameSettingsData
            {
                masterVolume = 1f,
                musicVolume = 1f,
                sfxVolume = 1f,
                sensitivity = 1f,
                gameLanguages = GameLanguages.English
            };
            SaveGameSettings(defaultData);
            return defaultData;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameSettingsData>(json);
    }

    //===========================================================
    //                     SESSION PROGRESSION
    //===========================================================
    public static void SaveProgression(SessionProgressionData data)
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "Progression/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string file = path + "SessionProgression.json";
        File.WriteAllText(file, JsonUtility.ToJson(data, true));
        Debug.Log("<color=green> SessionProgression salvo com sucesso!</color>");
    }

    public static SessionProgressionData LoadProgression()
    {
        string basePath = Application.persistentDataPath + "/Saves/";
        string path = basePath + "Progression/SessionProgression.json";
        if (!File.Exists(path))
        {
            Debug.Log("<color=yellow>Arquivo de Progressão não encontrado, criando defaults</color>");
            SessionProgressionData defaultData = new SessionProgressionData
            {
                CommonCollectablesCount = 0,
                HiddenCollectablesCount = 0,
                lastSceneName = SceneManager.GetSceneByBuildIndex(0).name
            };
            SaveProgression(defaultData);
            return defaultData;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SessionProgressionData>(json);
    }

    public static void ResetProgression()
    {
        SessionProgressionData defaultData = new SessionProgressionData
        {
            CommonCollectablesCount = 0,
            HiddenCollectablesCount = 0,
            lastSceneName = SceneManager.GetSceneByBuildIndex(0).name
        };
        SaveProgression(defaultData);
        Debug.Log("<color=green> SessionProgression resetado com sucesso!</color>");
    }
}
