using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;

    public float smoothTime;

    private Vector3 _vel;

    private void FixedUpdate()
    {
        var targetPos = followObject.position + followObject.forward * rotationOffset;
        targetPos.y = 0;
        transform.position = Vector3.SmoothDamp(transform.position,
            targetPos, ref _vel, smoothTime);
    }
}
