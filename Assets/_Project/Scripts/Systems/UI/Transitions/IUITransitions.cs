public interface IUITransition
{
    void PlayEnter(Panel panel, System.Action onComplete = null);
    void PlayExit(Panel panel, System.Action onComplete = null);
}