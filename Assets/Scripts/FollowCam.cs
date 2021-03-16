using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target = null;

    public float distance = 50f;
    public float rotationSpeed = 100f;

    Vector3 cameraPosition;
    Vector3 smoothPosition;

    public float smoothTime = 0.125f;
    float angle;

    void FixedUpdate() {
        cameraPosition = target.position - (target.forward * distance) + target.up * distance * 0.25f;
        smoothPosition = Vector3.Lerp(transform.position, cameraPosition, smoothTime);
        transform.position = smoothPosition;

        angle = Mathf.Abs(Quaternion.Angle(transform.rotation, target.rotation));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, (rotationSpeed + angle) * Time.deltaTime);
    }
}
