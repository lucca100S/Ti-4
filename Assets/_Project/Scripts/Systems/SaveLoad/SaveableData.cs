using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableData
{
    public List<SceneSaveData> sceneData = new List<SceneSaveData>();
}

[System.Serializable]
public class SceneSaveData
{
    public string sceneName;
    public PlayerSaveData playerData;
    public CollectableSaveData[] collectablesData;
    public CheckPointSaveData[] checkPointsData;
}

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 playerPosition;
}

[System.Serializable]
public class CollectableSaveData
{
    public string collectibleID;
    public bool isCollected;
}

[System.Serializable]
public class CheckPointSaveData
{
    public string checkPointID;
    public bool isActivated;
}
