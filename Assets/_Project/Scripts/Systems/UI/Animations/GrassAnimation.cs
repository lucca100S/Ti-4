using UnityEngine;
using DG.Tweening;

public class AnimationImagem : MonoBehaviour
{
    [Header("References")]
    public RectTransform img;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sfx;

    void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (audioSource != null && sfx != null)
            audioSource.PlayOneShot(sfx);

        img.anchoredPosition = new Vector2(img.anchoredPosition.x, -100f);
        img.sizeDelta = new Vector2(img.sizeDelta.x, 0f);

        img.DOAnchorPosY(0f, 0.4f).SetEase(Ease.OutCubic);
        img.DOSizeDelta(new Vector2(img.sizeDelta.x, 200f), 0.4f)
           .SetEase(Ease.OutBack);
    }
}