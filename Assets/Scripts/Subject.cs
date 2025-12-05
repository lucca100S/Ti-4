using System;
using UnityEngine;

public class Subject : MonoBehaviour
{
    public static Subject instance { get; private set; }
    public Action<int> collectedItem;
    public Action<int> totalScoreChanged;
    public Action playerHitObstacle;
    public Action checkpointActivated;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}