using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;

    private Vector3 cameraTransformPosition;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler Singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    
    public float minimumPivot = -40.0f;
    public float maximumPivot = 40.0f;

    private const float CameraSphereRadius = -0.2f;
    private const float CameraCollisionOffset = 0.2f;
    private const float MinimumCollisionOffset = 0.2f;

    private void Awake()
    {
        Singleton = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        var targetTransformPosition = Vector3.SmoothDamp
            (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetTransformPosition;
        
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        lookAngle += (mouseX * lookSpeed) / delta;
        pivotAngle -= (mouseY * pivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        var rotation = Vector3.zero;
        rotation.y = lookAngle;
        
        var targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPosition;
        var direction = cameraTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, CameraSphereRadius, direction, out var hit,
            Mathf.Abs(targetPosition), ignoreLayers))
        {
            var dist = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dist - CameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < MinimumCollisionOffset)
        {
            targetPosition = -MinimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
}
