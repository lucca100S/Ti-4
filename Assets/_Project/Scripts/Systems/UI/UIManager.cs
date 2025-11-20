using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject transitionPanel;
    public UnityEngine.Video.VideoPlayer transitionVideo;
    void Awake()
    {
        EventBus.Subscribe<ChangePanelEvent>(OnChangePanel);
                var videoTransition = new VideoWipeTransition(
            transitionPanel,
            transitionVideo,
            this
        );
    }

    void OnChangePanel(ChangePanelEvent eventData)
    {
        eventData.currentPanel.OnExit(eventData.targetPanel);
        eventData.targetPanel.OnEnter(eventData.currentPanel);
    }
}
