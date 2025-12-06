using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Collections;

public class CreditsPanelAnimator : MonoBehaviour
{
    [Header("References")]
    public RectTransform panel;
    public RectTransform leaves;

    [Header("Audio")]
    public AudioSource leafSound;
    public AudioSource openSound;
    public AudioSource closeSound;

    [Header("Animation Settings")]
    public float slideDuration = 0.6f;
    public float leavesDelay = 0.15f;
    public float leavesDuration = 0.4f;

    private Vector2 panelHiddenPos;
    private Vector2 panelShownPos;

    private Vector2 leavesHiddenPos;
    private Vector2 leavesShownPos;

    void Awake()
    {
        EventBus.Subscribe<ActivateSubPanelEvent>(OnActivateSubPanel);
        EventBus.Subscribe<ToggleSubPanelEvent>(OnToggleSubPanel);

        panelShownPos = panel.anchoredPosition;
        panelHiddenPos = panelShownPos + new Vector2(900f, 0f);

        leavesShownPos = leaves.anchoredPosition;
        leavesHiddenPos = leavesShownPos + new Vector2(900f, 0f);

        panel.anchoredPosition = panelHiddenPos;
        leaves.anchoredPosition = leavesHiddenPos;
    }

    void OnActivateSubPanel(ActivateSubPanelEvent evt)
    {
        if (evt.subPanel.gameObject == this.gameObject)
            Show();
    }

    void OnToggleSubPanel(ToggleSubPanelEvent evt)
    {
        if (evt.subPanel.gameObject == this.gameObject)
        {
            if (evt.activate) Show();
            else StartCoroutine(HideRoutine());
        }
    }

    void Show()
    {
        gameObject.SetActive(true);

        if (openSound != null)
        openSound.Play();

        panel.DOAnchorPos(panelShownPos, slideDuration)
            .SetEase(Ease.OutCubic);

        leaves.DOAnchorPos(leavesShownPos, leavesDuration)
            .SetDelay(leavesDelay)
            .SetEase(Ease.OutBack);
    }

    IEnumerator HideRoutine()
    {
        if (closeSound != null)
        closeSound.Play();

        panel.DOAnchorPos(panelHiddenPos, slideDuration)
            .SetEase(Ease.InCubic);

        Tween leavesTween = leaves.DOAnchorPos(leavesHiddenPos, leavesDuration)
            .SetDelay(leavesDelay)
            .SetEase(Ease.InCubic);

        yield return leavesTween.WaitForCompletion();
        gameObject.SetActive(false);
    }

    void Hide()
    {
        
    }
}