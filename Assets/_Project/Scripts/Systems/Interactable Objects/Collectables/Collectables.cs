using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Representa um colet vel no cen rio.
/// Notifica observadores quando   coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    [SerializeField] private float _spinDuration = 1;
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceDuration = 1;
    [SerializeField] private ParticleSystem _collectEffect;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;

    public CollectableType collectableType;
    public CollectableSaveData collectableSaveData;

    void Awake()
    {
        collectableSaveData.id = this.gameObject.name;
    }
    public void LoadData()
    {
        if (!collectableSaveData.isCollected)
        {
            Debug.Log("AHHHHHHHHHH");
            transform.DORotate(new Vector3(0, 360, 0), _spinDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
            transform.DOMoveY(transform.position.y + _bounceHeight, _bounceDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        }
        else
        {
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }

    public override void Interaction()
    {
        Debug.Log($"[Collectable] Coletado: {this.gameObject.name}");
        switch (collectableType)
        {
            case CollectableType.Common:
                EventBus.Publish(new AddCommonCollectableCountEvent());
                break;
            case CollectableType.Hidden:
                EventBus.Publish(new AddHiddenCollectableCountEvent());
                break;
        }
        CollectableObservable.Instance?.NotifyListeners(this);
        _collider.enabled = false;
        _renderer.enabled = false;
        _collectEffect.Play();
        transform.DOKill();
        AudioPlayer.Play(AudioId.CollectablePickUp);
        collectableSaveData.isCollected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collectableSaveData.isCollected)
        {
            Interaction();
        }
    }
}

public enum CollectableType
{
    Common, Hidden
}