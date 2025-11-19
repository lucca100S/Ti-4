using Lugu.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonObserver : MonoBehaviour
{
    public Action OnPoisonCleanTrigger;
    public Action OnPoisonCleanStart;
    public Action OnPoisonCleanEnd;
    [SerializeField] private float _cleanDelay;
    [SerializeField] private float _cleanDuration;
    [SerializeField] private List<PoisonListener> _poisonListeners;

    private bool _isCleaned = false;

    private void Start()
    {
        foreach (PoisonListener poisonListener in _poisonListeners)
        {
            OnPoisonCleanTrigger += poisonListener.CleanTrigger;
            OnPoisonCleanStart += poisonListener.CleanStart;
            OnPoisonCleanEnd += poisonListener.CleanEnd;
        }
    }

    private IEnumerator CleanTrigger()
    {
        OnPoisonCleanTrigger?.Invoke();
        yield return new WaitForSeconds(_cleanDelay);
        OnPoisonCleanStart?.Invoke();
        yield return new WaitForSeconds(_cleanDuration);
        OnPoisonCleanEnd?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_isCleaned)
        {
            StartCoroutine(CleanTrigger());
            _isCleaned = true;
        }
    }
}
