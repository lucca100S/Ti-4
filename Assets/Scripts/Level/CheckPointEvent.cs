using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CheckPointEvent : MonoBehaviour
{
    public CheckPoint[] requiredCheckpoints;
    public CheckPoint[] prohibitedCheckpoints;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        Subject.instance.checkpointActivated += CheckIfUnlockable;
    }

    void OnDisable()
    {
        Subject.instance.checkpointActivated -= CheckIfUnlockable;
    }

    void CheckIfUnlockable()
    {
        bool canUnlock = true;

        foreach (CheckPoint checkPoint in requiredCheckpoints)
        {
            if (!checkPoint.activated)
            {
                canUnlock = false;
            }
        }

        foreach (CheckPoint checkPoint in prohibitedCheckpoints)
        {
            if (checkPoint.activated)
            {
                canUnlock = false;
            }
        }

        if (canUnlock) UnlockDoor();
    }
    
    void UnlockDoor()
    {
        Debug.Log("Unlock");
        animator.SetTrigger("open");
    }
}
