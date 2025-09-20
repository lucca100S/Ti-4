using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSpawnpoint : MonoBehaviour
{
    private Vector3 _originalSpawnPoint;
    private Vector3 _spawnPoint;
    private CharacterController _characterController;
    [SerializeField] private float _killZoneY;

    [SerializeField] private List<Transform> _teleportPoints;

    #region Properties

    public Vector3 spawnPoint
    {
        get { return _spawnPoint; }
        set { _spawnPoint = value; }
    }

    #endregion

    private void Awake()
    {
        _originalSpawnPoint = transform.position;
        _spawnPoint = _originalSpawnPoint;
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
        Teleport(spawnPoint);
    }

    private void Teleport(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
    }

    public void TeleportPoint(int point)
    {
        if(point < _teleportPoints.Count)
        {
            if (_teleportPoints[point] != null)
            {
                Teleport(_teleportPoints[point].position);
            }
        }
    }

    public void SetSpawnPoint(Vector3 pos)
    {
        _spawnPoint = pos;
    }

    public void ResetSpawnPoint()
    {
        SetSpawnPoint(_originalSpawnPoint);
    }
}
