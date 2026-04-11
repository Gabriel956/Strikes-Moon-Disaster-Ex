using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Transform target;
    public Vector3 offset;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private bool lockCursorWhileRotating = true;

    private float yaw;
    private float pitch;
    private float distance;

    void Awake()
    {
        distance = offset.magnitude;
        if (distance <= 0.001f)
        {
            distance = 5f;
            offset = new Vector3(0f, 0f, distance);
        }

        Vector3 toCamera = offset.normalized;
        yaw = Mathf.Atan2(toCamera.x, toCamera.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(Mathf.Clamp(toCamera.y, -1f, 1f)) * Mathf.Rad2Deg;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        bool isRotating = Mouse.current != null && Mouse.current.rightButton.isPressed;
        if (isRotating)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            yaw += delta.x * rotationSpeed;
            pitch -= delta.y * rotationSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        if (lockCursorWhileRotating)
        {
            Cursor.lockState = isRotating ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isRotating;
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        offset = rotation * Vector3.forward * distance;
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
