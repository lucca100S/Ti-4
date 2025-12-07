using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private bool _keepYRotation = true;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera != null)
        {
            Vector3 direction = transform.position - _camera.transform.position;
            if (_keepYRotation)
            {
                direction.y = 0; // Keep only the horizontal direction
            }
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
