using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Tower;

    public RadialRope Rope;

    public float MinOffset = 12;

    public float MaxOffset = 15;

    private void LateUpdate()
    {
        var ropePosition = Rope.transform.position;

        var towerCenter = new Vector3(Tower.position.x, ropePosition.y * 2, Tower.position.z);

        var dynamicOffset = Mathf.Min(MinOffset + (Rope.Player1.transform.position - Rope.Player2.transform.position).magnitude, MaxOffset);

        transform.position = towerCenter + (ropePosition - towerCenter).normalized * ((ropePosition - towerCenter).magnitude + dynamicOffset);
        transform.LookAt(ropePosition);
    }
}