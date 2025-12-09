using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
public class CountCollectables : MonoBehaviour
{
    [Header("Counters")]
    const int stageCount = 3;
    public static int[] common = new int[stageCount];
    public static int[] hidden = new int[stageCount];
    const string fase1 = "Level1Entrega";
    const string fase2 = "LuguFase2";
    const string fase3 = "LuguFase3";
    void Awake()
    {
        ResetFaseCounter(SceneManager.GetActiveScene().name);
        EventBus.Subscribe<AddCommonCollectableCountEvent>(AddCollectableCommon);
        EventBus.Subscribe<AddHiddenCollectableCountEvent>(AddCollectAbleHidden);
    }

    void AddCollectAbleHidden(AddHiddenCollectableCountEvent evt)
    {
        Debug.Log("Adding Collectable");
        switch (SceneManager.GetActiveScene().name)
        {
            case fase1:
                CountCollectables.hidden[0] = CountCollectables.hidden[0]+1;
                Debug.Log(hidden[0] + "New number of collected");
                break;
            case fase2:
                CountCollectables.hidden[1] = CountCollectables.hidden[1]+1;
                Debug.Log(hidden[1] + "New number of collected");
                break;
            case fase3:
                CountCollectables.hidden[2] = CountCollectables.hidden[2]+1;
                Debug.Log(hidden[2] + " New number of collected");
                break;
            default:
                Debug.Log("Error on adding hidden collectable");
                break;
        }
    }
    void AddCollectableCommon(AddCommonCollectableCountEvent evt)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case fase1:
                CountCollectables.common[0] = CountCollectables.common[0]+1;
                Debug.Log(common[0] + "New number of collected");
                break;
            case fase2:
                CountCollectables.common[1] = CountCollectables.common[1]+1;
                Debug.Log(common[1] + "New number of collected");
                break;
            case fase3:
                CountCollectables.common[2] = CountCollectables.common[2]+1;
                Debug.Log(common[2] + " New number of collected");
                break;
            default:
                Debug.Log("Error on adding common collectable");
                break;
        }
    }
    public static void ResetAll()
    {
        ResetFaseCounter(fase1);
        ResetFaseCounter(fase2);
        ResetFaseCounter(fase3);
    }

    public static void ResetFaseCounter(string fase)
    {
        int index;
        switch (fase)
        {
            case fase1:
                index = 0;
                break;
            case fase2:
                index = 1;
                break;
            case fase3:
                index = 2;
                break;
            default:
                Debug.Log("Error on index setting");
                index = 4;
                break;
        }
        if (index != 4)
        {
            Debug.Log($"Reset of collectables stage {SceneManager.GetActiveScene().name}");
            common[index] = 0;
            hidden[index] = 0;
        }
    }

    public static int CollectedNumberByType(CollectableType collectableType)
    {
        Debug.Log("Counting");
        int[] array = new int[stageCount];
        int counter = 0;
        switch (collectableType)
        {
            case CollectableType.Common:
                array = common;
                break;
            case CollectableType.Hidden:
                array = hidden;
                break;
        }

        foreach (int collectable in array)
        {
            counter += collectable;
        }
        Debug.Log(counter);
        return counter;
    }
}


