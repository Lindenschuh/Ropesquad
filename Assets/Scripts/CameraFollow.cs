using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Camera _cam;

    private Vector3 lastPosition;

    private void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        transform.LookAt(target);
    }
}