using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementManger
{
    public static void NextPosition(Transform target, float hInput, float speed, Tower center, float characterOffset, ref float lookDir)
    {
        var towerCenter = new Vector3(center.transform.position.x, target.position.y, center.transform.position.z);
        var playerRadius = center.Radius + characterOffset;

        target.RotateAround(center.transform.position, Vector3.up, hInput * speed * Time.fixedDeltaTime);

        if (hInput != 0)
            lookDir = (hInput > 0) ? 0 : 180;

        Vector3 tangentVector = Quaternion.Euler(0, lookDir, 0) * (target.position - towerCenter);
        target.rotation = Quaternion.LookRotation(tangentVector);

        // Check if inside Radius

        var distanceSqr = (target.position - towerCenter).sqrMagnitude;
        float radiusSqr = center.Radius * center.Radius;

        if (distanceSqr != radiusSqr)
        {
            target.position = towerCenter + (target.position - towerCenter).normalized * playerRadius;
        }
    }
}