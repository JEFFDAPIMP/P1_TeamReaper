using UnityEngine;

public class FaceCameraWithParentRotation : MonoBehaviour
{
    public Transform parent; // Assign your parent object's transform in the inspector
    public Camera cam; // Assign your camera's transform in the inspector

    private void Awake()
    {
        cam = Camera.main;
        parent = GetComponentInParent<Transform>();
    }

    /*
    void LateUpdate()
    {
        // Look at the camera
        transform.LookAt(cam.transform);

        // Inherit parent's rotation
        transform.rotation = parent.rotation * transform.rotation;
    }
    */

    /*
    void LateUpdate()
    {
        // Keep sprite facing the camera
        transform.up = (cam.transform.position - transform.position).normalized;

        // Inherit parent's rotation
        transform.rotation = parent.rotation;
    }
    */

    void LateUpdate()
    {
        // Keep sprite facing the camera
        Vector3 direction = cam.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Inherit parent's rotation
        transform.rotation *= parent.rotation;
    }
}