using UnityEngine;

public class PlayerDropShadow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private SpriteRenderer _visuals;

    [SerializeField] private float _maxSizeDistance = 0.2f;
    [SerializeField] private float _minSizeDistance = 5f;

    [SerializeField] private float _maxSize = 1f;
    [SerializeField] private float _minSize = 0.2f;

    private const float SPHERE_CAST_RADIUS = 0.3f;
    private const float SHADOW_Y_OFFSET = 0.1f;

    private void Start()
    {
        transform.parent = null;
    }

    private void LateUpdate()
    {

        Vector3 targetPosition = GetDropPosition();

        transform.position = targetPosition;
    }

    private Vector3 GetDropPosition()
    {
        if (_target != null)
        {
            Vector3 targetPosition = _target.transform.position;


            if (Physics.SphereCast(targetPosition + _target.transform.up * SPHERE_CAST_RADIUS, SPHERE_CAST_RADIUS, -_target.transform.up, out RaycastHit hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Player")))
            {
                _visuals.enabled = true;
                targetPosition.y = hitInfo.point.y + SHADOW_Y_OFFSET;

                transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_target.transform.forward, hitInfo.normal), hitInfo.normal);

                ChangeShadowSize(hitInfo.distance);
            }
            else
            {
                _visuals.enabled = false;
            }

            return targetPosition;
        }
        return transform.position;
    }

    private void ChangeShadowSize(float distance)
    {
        float t = Mathf.InverseLerp(_maxSizeDistance, _minSizeDistance, distance);
        float newSize = Mathf.Lerp(_maxSize, _minSize, t);
        transform.localScale = new Vector3(newSize, newSize, newSize);
    }

}
