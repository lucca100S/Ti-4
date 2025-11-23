using UnityEngine;
using System.Collections;

public class FadeTransition : IUITransition
{
    private float duration;

    public FadeTransition(float duration = 1f)
    {
        this.duration = duration;
    }

    public void PlayEnter(Panel panel, System.Action onComplete = null)
    {
        panel.gameObject.SetActive(true);
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        panel.GetComponent<MonoBehaviour>().StartCoroutine(Fade(canvasGroup, 1f, onComplete));
    }

    public void PlayExit(Panel panel, System.Action onComplete = null)
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();

        panel.GetComponent<MonoBehaviour>().StartCoroutine(Fade(canvasGroup, 0f, () =>
        {
            panel.gameObject.SetActive(false);
            onComplete?.Invoke();
        }));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float target, System.Action onComplete)
    {
        float start = canvasGroup.alpha;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, timer / duration);
            yield return null;
        }
        canvasGroup.alpha = target;
        onComplete?.Invoke();
    }
}
