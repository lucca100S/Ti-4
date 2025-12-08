using UnityEngine;

public class SaveCollision : MonoBehaviour
{
    public SaveHandler saveHandler;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            saveHandler.SaveSceneNow();
        }
    }
}
