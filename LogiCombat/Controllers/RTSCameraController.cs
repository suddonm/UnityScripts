using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float panSpeed = 20f;          // Speed of camera movement
    public float panBorderThickness = 10f; // Border thickness for screen-edge panning
    public Vector2 upperPanLimit;             // Limits for panning (X, Z)
    public Vector2 lowerPanLimit;             // Limits for panning (X, Z)
    public float zoomSpeed = 20f;        // Speed of zooming
    public float minY = 10f;             // Minimum height for the camera
    public float maxY = 100f;            // Maximum height for the camera
    public float rotationSpeed = 50f;    // Speed for rotating the camera

    void Update()
    {
        Vector3 pos = transform.position;

       
        // Keyboard panning (WASD or arrow keys)
        if (Input.GetKey("w") ||
            GameManager.Instance.isEdgePanningEnabled && Input.mousePosition.y >= Screen.height - panBorderThickness)
            pos.z += panSpeed * Time.deltaTime;

        if (Input.GetKey("s") ||
            GameManager.Instance.isEdgePanningEnabled && Input.mousePosition.y <= panBorderThickness)
            pos.z -= panSpeed * Time.deltaTime;

        if (Input.GetKey("d") ||
            GameManager.Instance.isEdgePanningEnabled && Input.mousePosition.x >= Screen.width - panBorderThickness)
            pos.x += panSpeed * Time.deltaTime;

        if (Input.GetKey("a") ||
            GameManager.Instance.isEdgePanningEnabled && Input.mousePosition.x <= panBorderThickness)
            pos.x -= panSpeed * Time.deltaTime;

        // Zooming in/out with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;

        // Clamp the camera's position within boundaries
        pos.x = Mathf.Clamp(pos.x, lowerPanLimit.x, upperPanLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, lowerPanLimit.y, upperPanLimit.y);

        // Camera rotation
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        transform.position = pos;
    }
}
