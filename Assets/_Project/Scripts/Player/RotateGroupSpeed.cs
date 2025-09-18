using System.Collections.Generic;
using UnityEngine;
using Player.Movement;

public class RotateGroupSpeed : MonoBehaviour
{

    [SerializeField] private List<GameObject> m_objects;
    [SerializeField] private PlayerMovement m_target;

    [SerializeField] private float m_rotationSpeed = 10f;
    [SerializeField] private Vector3 m_minAngle = Vector3.zero;
    [SerializeField] private Vector3 m_maxAngle = new Vector3(90, 0, 0);
    [SerializeField] private Vector3 m_angleMultiplier = new Vector3(6, 1, 3);
    [SerializeField] private float m_angleDampeming = 5f;
    [SerializeField] private Vector3 m_direction = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        if (m_target != null)
        {
            Vector3 movement = (m_target.transform.right * m_target.force.x) + 
                (m_target.transform.up * -m_target.force.y) + 
                (m_target.transform.forward * m_target.force.z);

            //movement.z *= m_target.direction.z;

            Vector3 rotation = Vector3.one;
            rotation.x *=
                Mathf.Abs(movement.x * m_direction.x * m_angleMultiplier.x)+
                (movement.y * m_direction.y * m_angleMultiplier.y) +
                Mathf.Abs(movement.z * m_direction.z * m_angleMultiplier.z);
            rotation.y *= 0;
            rotation.z *= 0;

            rotation.x = Mathf.Clamp(rotation.x, m_minAngle.x, m_maxAngle.x);
            rotation.y = Mathf.Clamp(rotation.y, m_minAngle.y, m_maxAngle.y);
            rotation.z = Mathf.Clamp(rotation.z, m_minAngle.z, m_maxAngle.z);

            for (int i = 0; i < m_objects.Count; i++)
            {
                GameObject obj = m_objects[i];

                if (obj != null)
                {
                    float dampenFactor = (m_angleDampeming * (i == 0 ? 1 : i));
                    obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(rotation / dampenFactor), Time.deltaTime * m_rotationSpeed / dampenFactor);
                }
            }
        }
    }

}
