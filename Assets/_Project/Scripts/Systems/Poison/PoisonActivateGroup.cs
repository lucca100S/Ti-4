using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonActivateGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjectsToActivate = new List<GameObject>();

    [SerializeField] private float _duration = 1;

    public void Activate()
    {
        StartCoroutine(ChangeState(true));
    }

    public void Disactivate()
    {
        StartCoroutine(ChangeState(false));
    }

    public void Toggle()
    {
        StartCoroutine(ChangeState(!_gameObjectsToActivate[0].activeSelf));
    }

    private IEnumerator ChangeState(bool active)
    {
        float count = _gameObjectsToActivate.Count;
        for (int i = 0; i < count; i++)
        {
            _gameObjectsToActivate[i].SetActive(active);
            yield return new WaitForSeconds(_duration / count);
        }
    }
}
