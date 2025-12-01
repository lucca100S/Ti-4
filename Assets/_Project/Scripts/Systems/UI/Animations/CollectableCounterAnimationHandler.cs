using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class CollectableCounterAnimationHandler : MonoBehaviour
{
    public Animation animationComponent;
    public AnimationClip revealAnimation;
    public AnimationClip hideAnimation;
    bool isActive = false;
    [SerializeField]
    float animationDuration = 1.0f;
    public CollectableCounterHandler collectableCounterHandler;
    public void Awake()
    {
        // Subscribe to the AddCollectableCountEvent
        EventBus.Subscribe<AddCollectableCountEvent>(OnAddCollectableCount);
    }
    private void OnAddCollectableCount(AddCollectableCountEvent evt)
    {
        Debug.Log("Collectable Count Added Event Received");
        if(isActive)
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
    private void PlayCollectableCounterAnimationHide()
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
