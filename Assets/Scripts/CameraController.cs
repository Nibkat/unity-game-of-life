using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private float zOffset = -10f;
    [SerializeField]
    private float scrollSpeed = 2f;
    [SerializeField]
    private Vector2 orthographicSizeMinMax = new Vector2(5, 22);

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position = new Vector3(transform.position.x + -Input.GetAxis("Mouse X"), transform.position.y + -Input.GetAxis("Mouse Y"), zOffset);
        }

        float _orthographicSize = Mathf.Clamp(_camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, orthographicSizeMinMax.x, orthographicSizeMinMax.y);
        _camera.orthographicSize = _orthographicSize;
    }
}
