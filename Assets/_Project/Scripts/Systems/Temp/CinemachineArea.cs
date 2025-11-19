using UnityEngine;

public class CinemachineArea : MonoBehaviour
{
    [SerializeField] private GameObject m_cinemachine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_cinemachine.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_cinemachine.SetActive(false);
        }
    }
}
