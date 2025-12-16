using DG.Tweening;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;

    public float smoothTime;

    private Vector3 _vel;

    public Camera mainCamera;
    private Vector3 _originalCameraPos;

    private void Start()
    {
        _originalCameraPos = mainCamera.transform.localPosition;
    }

    private void FixedUpdate()
    {
        var targetPos = followObject.position + followObject.forward * rotationOffset;
        targetPos.y = 0;
        transform.position = Vector3.SmoothDamp(transform.position,
            targetPos, ref _vel, smoothTime);
    }

    public void ShakeCamera(float magnitude, float duration)
    {
        mainCamera.transform.DOKill();
        mainCamera.transform.localPosition = _originalCameraPos;
        mainCamera.DOShakePosition(duration, magnitude);
    }
}
