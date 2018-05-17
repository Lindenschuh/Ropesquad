using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroudableObject : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public Vector3 GroundPoint { get; private set; }
    public float DstToGround;

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (IsGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, DstToGround))
            GroundPoint = hit.point;
    }
}