using System;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Speed for panning
    public float scrollSpeed = 1000f; // Speed for zooming in/out
    public float rotationSpeed = 100f; // Speed for rotating the camera
    public Vector2 panLimit; // Limits to how far you can pan
    public float minY = 10f, maxY = 80f; // Limits for zooming

    public Transform target; // The selected unit to follow
    public float smoothSpeed = 0.125f; // Speed of camera movement
    public Vector3 offset; // Offset to maintain a fixed distance from the unit

    public enum CameraMode
    {
        RTS = 0,
        Follow = 1
    }

    public CameraMode cameraMode = CameraMode.RTS;

    private void LateUpdate()
    {
        if (cameraMode == CameraMode.Follow && target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Optionally rotate the camera to look at the target
            transform.LookAt(target);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    internal void SwitchCameraMode()
    {
        if (cameraMode == CameraMode.RTS)
        {
            SetCameraModeFollow();
        }
        else
        {
            SetCameraModeRTS();
        }
    }

    internal void SetCameraModeFollow()
    {
        cameraMode = CameraMode.Follow;
    }

    internal void SetCameraModeRTS()
    {
        cameraMode = CameraMode.RTS;
    }

    void Update()
    {
        if (cameraMode == CameraMode.RTS)
        {
            HandleMovement();
            HandleZoom();
            HandleRotation();
        }
    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;

        // WASD or arrow key panning
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        // Clamp the camera within the pan limits
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;

        // Adjust the camera's height based on scroll input
        pos.y -= scroll * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button to rotate
        {
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");

            // Rotate around the Y-axis
            transform.Rotate(Vector3.up, rotateHorizontal * rotationSpeed * Time.deltaTime, Space.World);

            // Tilt the camera up and down, limit the vertical rotation
            transform.Rotate(Vector3.right, -rotateVertical * rotationSpeed * Time.deltaTime);
        }
    }
}
