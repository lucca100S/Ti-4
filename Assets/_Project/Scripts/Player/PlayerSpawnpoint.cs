using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnpoint : MonoBehaviour
{
    private Vector3 _originalSpawnPoint;
    private Vector3 _spawnPoint;
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
        transform.position = position;
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
