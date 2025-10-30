using System.Collections.Generic;
using UnityEngine;
using Player.Movement;
using Player;

public class RotateGroupSpeed : MonoBehaviour
{

    [SerializeField] private List<GameObject> _objects;
    [SerializeField] private PlayerStateMachine _target;

    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Vector3 _minAngle = Vector3.zero;
    [SerializeField] private Vector3 _maxAngle = new Vector3(90, 0, 0);
    [SerializeField] private Vector3 _angleMultiplier = new Vector3(6, 1, 3);
    [SerializeField] private float _angleDampeming = 5f;
    [SerializeField] private Vector3 _direction = Vector3.up;

    private List<Vector3> _originalRotations = new List<Vector3>();

    private void Awake()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            GameObject obj = _objects[i];
            if (obj != null)
            {
                _originalRotations.Add(obj.transform.localEulerAngles);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            Vector3 force = _target.RigidBody.linearVelocity;
            Vector3 movement = (_target.transform.right * force.x) +
                (_target.transform.up * -force.y) +
                (_target.transform.forward * force.z);

            //movement.z *= m_target.direction.z;

            Vector3 rotation = Vector3.one;
            rotation.x *=
                Mathf.Abs(movement.x * _direction.x * _angleMultiplier.x) +
                (movement.y * _direction.y * _angleMultiplier.y) +
                Mathf.Abs(movement.z * _direction.z * _angleMultiplier.z);
            rotation.y *= 0;
            rotation.z *= 0;

            rotation.x = Mathf.Clamp(rotation.x, Mathf.Min(_minAngle.x, _maxAngle.x), Mathf.Max(_minAngle.x, _maxAngle.x));
            rotation.y = Mathf.Clamp(rotation.y, Mathf.Min(_minAngle.y, _maxAngle.y), Mathf.Max(_minAngle.y, _maxAngle.y));
            rotation.z = Mathf.Clamp(rotation.z, Mathf.Min(_minAngle.z, _maxAngle.z), Mathf.Max(_minAngle.z, _maxAngle.z));


            for (int i = 0; i < _objects.Count; i++)
            {
                GameObject obj = _objects[i];

                if (obj != null)
                {
                    float dampenFactor = (_angleDampeming * (i == 0 ? 1 : i));
                    obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler((rotation / dampenFactor) + _originalRotations[i]), Time.deltaTime * _rotationSpeed / dampenFactor);
                }
            }
        }
    }

}
