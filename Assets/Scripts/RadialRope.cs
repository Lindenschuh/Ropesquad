using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRope : MonoBehaviour
{
    public float Radius;
    public float MinRadius;
    public float MaxRadius;
    public PlayerControlls Player1;
    public PlayerControlls Player2;

    public float offsetY;

    public void BoundTransform(PlayerControlls player)
    {
        Vector3 centerPosition;
        float localRadius;
        if (!player.IsAnchord)
        {
            centerPosition = transform.localPosition - Vector3.up * offsetY;
            localRadius = Radius;
        }
        else
        {
            centerPosition = player.transform.position - ((player.transform.position - player.Anchor.position) / 2);
            localRadius = 5f;
        }

        float distance = Vector3.Distance(player.transform.position, centerPosition);

        if (distance > localRadius)
        {
            Vector3 fromOriginToObject = player.transform.position - centerPosition;
            fromOriginToObject *= localRadius / distance;
            player.transform.position = centerPosition + fromOriginToObject;
        }
    }

    public void changeRopeLength(float deltaLength)
    {
        Radius = Mathf.Clamp(Radius + deltaLength, MinRadius, MaxRadius);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = (Player1.transform.position -
            (Player1.transform.position - Player2.transform.position) / 2)
            + Vector3.up * offsetY;

        BoundTransform(Player1);
        BoundTransform(Player2);
    }
}