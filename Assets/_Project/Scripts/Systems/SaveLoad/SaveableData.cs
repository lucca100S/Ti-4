
using UnityEngine;
using System.Collections.Generic;

//Collectables
[System.Serializable]
public class CollectableSaveData
{
    public string id;
    public bool isCollected; //Default: false
}
//Checkpoint
[System.Serializable]
public class CheckPointSaveData
{
    public string id;
    public bool isActivated; //Default: false
}
//Player
[System.Serializable]
public class PlayerSaveData
{
    public Vector3 startPosition; //Default: position of the player at the beginning of the game)
}
//GameSettings
[System.Serializable]
public class GameSettingsData
{
    public float masterVolume; //Default: 1
    public float musicVolume; //Default: 1
    public float sfxVolume; //Default: 1
    public float sensitivity; //Default: 1
    public GameLanguages gameLanguages; //Default: GameLanguages.English
}
//Progression
[System.Serializable]
public class SessionProgressionData
{
    public int CommonCollectablesCount; //Default: 0
    public int HiddenCollectablesCount; //Default: 0
}

//Scene
[System.Serializable]
public class SceneSaveData
{
    public PlayerSaveData playerSaveData;
    public List<CollectableSaveData> collectablesSaveData;
    public List<CheckPointSaveData> checkPointsSaveData;
}

