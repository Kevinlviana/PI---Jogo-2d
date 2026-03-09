using UnityEngine;

// CameraFollow.cs
// Attach to: Main Camera
// Smoothly follows the player; optional X/Y clamping to prevent showing outside level

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public float smoothSpeed = 6f;
    public Vector3 offset = new Vector3(0, 1f, -10f);

    [Header("Bounds Clamping (optional)")]
    public bool useBounds = false;
    public float minX = -10f, maxX = 50f;
    public float minY = -5f,  maxY = 20f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        if (useBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minX, maxX);
            desired.y = Mathf.Clamp(desired.y, minY, maxY);
        }

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}
