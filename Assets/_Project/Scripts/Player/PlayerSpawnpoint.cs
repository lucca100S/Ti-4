using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSpawnpoint : MonoBehaviour
{
    private Vector3 _spawnPoint;
    private CharacterController _characterController;
    [SerializeField] private float _killZoneY;

    #region Properties

    public Vector3 spawnPoint
    {
        get { return _spawnPoint; }
        set { _spawnPoint = value; }
    }

    #endregion

    private void Awake()
    {
        _spawnPoint = transform.position;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(transform.position.y < _killZoneY)
        {
            ReturnToSpawnpoint();
        }
    }

    public void ReturnToSpawnpoint()
    {
        _characterController.enabled = false;
        transform.position = spawnPoint;
        _characterController.enabled = true;
    }
}
