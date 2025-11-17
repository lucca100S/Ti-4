using UnityEngine;
using UnityEngine.EventSystems;

public class SelectFirst : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
