using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BaseButtonEffects : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [Header("Effects")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    public float animSpeed = 0.15f;

    [Header("Audio")]
    public AudioSO hoverSound;
    public AudioSO clickSound;

    private Vector3 defaultScale;
    private Color defaultColor;
    private Image img;
    private Coroutine scaleCoroutine;

    void Awake()
    {
        img = GetComponent<Image>();
        defaultColor = img.color;
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) => Hover();
    public void OnSelect(BaseEventData eventData) => Hover();

    public void OnPointerExit(PointerEventData eventData) => Unhover();
    public void OnDeselect(BaseEventData eventData) => Unhover();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound) AudioPlayer.Play(clickSound);
    }

    private void Hover()
    {
        if (hoverSound) AudioPlayer.Play(hoverSound);
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(AnimateScale(hoverScale));
    }

    private void Unhover()
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(AnimateScale(defaultScale));
        img.color = defaultColor;
    }

    private IEnumerator AnimateScale(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, animSpeed);
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localScale = target;
        scaleCoroutine = null;
    }
}
