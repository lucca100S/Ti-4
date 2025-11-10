using UnityEngine;
public class LeavesHooverEffectHandler : UIComponentBase
{
    public GameObject rightLeaf;
    public GameObject leftLeaf;
    void Awake()
    {
        EventBus.Subscribe<LeavesHooverEfectMainMenu>(Behave);
    }
    private void Start()
    {
        rightLeaf.SetActive(false);
        leftLeaf.SetActive(false);
    }

    void Behave(LeavesHooverEfectMainMenu evt)
    {
        Debug.Log("Leaves Hoover Event Received");
        if (evt.active)
        {
            this.transform.position = new Vector3(evt.position.x, evt.position.y, 0);
            rightLeaf.SetActive(true);
            leftLeaf.SetActive(true);
        }
        else
        {
            this.transform.position = new Vector3(0, 0, 0);
            rightLeaf.SetActive(false);
            leftLeaf.SetActive(false);
        }
    }
}
