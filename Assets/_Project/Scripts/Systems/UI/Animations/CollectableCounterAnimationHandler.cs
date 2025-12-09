using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class CollectableCounterAnimationHandler : MonoBehaviour
{
    public Animation animationComponent;
    public CollectableType collectableTypeDisplayed;
    public AnimationClip revealAnimation;
    public AnimationClip hideAnimation;
    public bool isActive = false;
    [SerializeField]
    float animationDuration = 1.0f;
    public CollectableCounterHandler collectableCounterHandler;
    //public SaveHandler saveHandler;
    public void Start()
    {
        switch (collectableTypeDisplayed)
        {
            case CollectableType.Common:
                EventBus.Subscribe<AddCommonCollectableCountEvent>(OnAddCommonCollectableCount);
                collectableCounterHandler.collectableCount = CountCollectables.CollectedNumberByType(CollectableType.Common);
                break;
            case CollectableType.Hidden:
                EventBus.Subscribe<AddHiddenCollectableCountEvent>(OnAddHiddenCollectableCount);
                collectableCounterHandler.collectableCount = CountCollectables.CollectedNumberByType(CollectableType.Hidden);
                break;
        }

    }
    private void OnAddCommonCollectableCount(AddCommonCollectableCountEvent evt)
    {
        Debug.Log("Collectable Count Added Event Received");
        if (isActive)
        {
            collectableCounterHandler.UpdateCollectableText();
            return;
        }
        // Trigger the collectable counter animation
        PlayCollectableCounterAnimationReveal();
    }
    private void OnAddHiddenCollectableCount(AddHiddenCollectableCountEvent evt)
    {
        Debug.Log("Collectable Count Added Event Received");
        if (isActive)
        {
            collectableCounterHandler.UpdateCollectableText();
            return;
        }
        // Trigger the collectable counter animation
        PlayCollectableCounterAnimationReveal();
    }
    private void PlayCollectableCounterAnimationReveal()
    {
        isActive = true;
        // Animation logic here
        Debug.Log("Playing Collectable Counter Animation Reveal");
        this.animationComponent.Play(revealAnimation.name);
        StartCoroutine(WaitTimeToIncreaseCounter(.1f));
        StartCoroutine(WaitTimeDoDeactivate(animationDuration)); // Assuming 1 second animation duration
    }
    public void PlayCollectableCounterAnimationHide()
    {
        isActive = false;
        // Animation logic here
        Debug.Log("Playing Collectable Counter Animation Hide");
        this.animationComponent.Play(hideAnimation.name);
    }
    private System.Collections.IEnumerator WaitTimeDoDeactivate(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayCollectableCounterAnimationHide();
    }

    private System.Collections.IEnumerator WaitTimeToIncreaseCounter(float duration = 1.0f)
    {
        yield return new WaitForSeconds(duration);
        collectableCounterHandler.UpdateCollectableText();
    }
}

