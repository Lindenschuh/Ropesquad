using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Tower;

    public Transform Rope;

    public float CameraOffset;

    private void LateUpdate()
    {
        var ropePosition = Rope.position;

        var towerCenter = new Vector3(Tower.position.x, ropePosition.y * 2, Tower.position.z);

        transform.position = towerCenter + (ropePosition - towerCenter).normalized * ((ropePosition - towerCenter).magnitude + CameraOffset);
        transform.LookAt(ropePosition);
    }
}