using Unity.VisualScripting;
using UnityEngine;

public class RotateTowardsVelocity : MonoBehaviour
{

    Vector3 _previousPosition;


    private void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = _previousPosition.y;

        if(currentPosition != _previousPosition)
        {
            Vector3 movementDirection = (currentPosition - _previousPosition).normalized;

            transform.rotation = Quaternion.LookRotation(movementDirection, transform.up);

        }
    }

    private void LateUpdate()
    {
        _previousPosition = transform.position;
    }
}
