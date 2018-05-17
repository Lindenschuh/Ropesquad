using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadialRope : MonoBehaviour
{
    public float Radius;
    public float MinRadius;
    public float MaxRadius;
    public PlayerControlls Player1;
    public PlayerControlls Player2;

    public float offsetY;

    private float avdPl1;
    private float avdPl2;
    private float subDist;

    private Transform ankerPlayer1;
    private Transform ankerPlayer2;

    private LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    public void BoundTransform()
    {
        Vector3 positionP1 = Player1.transform.position + Vector3.up * offsetY;
        Vector3 positionP2 = Player2.transform.position + Vector3.up * offsetY;

        if (ankerPlayer1 == null)
        {
            ankerPlayer1 = transform;
            avdPl1 = (positionP1 - ankerPlayer1.position).magnitude;
        }

        if (ankerPlayer2 == null)
        {
            ankerPlayer2 = transform;
            avdPl2 = (positionP2 - ankerPlayer2.position).magnitude;
        }

        subDist = (ankerPlayer1.position - ankerPlayer2.position).magnitude;

        float fullDistance = Radius * 2;
        float remainingDistance = fullDistance - subDist;
        float availableDistance = remainingDistance - avdPl1 - avdPl1;

        float radiusP1 = (avdPl1 + availableDistance / 2) / 2;
        float radiusP2 = (avdPl2 + availableDistance / 2) / 2;

        Vector3 centerPositionP1 = positionP1 - ((positionP1 - ankerPlayer1.position) / 2);
        Vector3 centerPositionP2 = positionP2 - ((positionP2 - ankerPlayer2.position) / 2);

        if (!Player1.IsHolding)
            ApplyBoundries(Player1.transform, radiusP1, centerPositionP1);
        if (!Player2.IsHolding)
            ApplyBoundries(Player2.transform, radiusP2, centerPositionP2);
    }

    private void ApplyBoundries(Transform target, float radius, Vector3 center)
    {
        center = center - Vector3.up * offsetY;
        float distance = Vector3.Distance(target.position, center);

        if (distance > radius)
        {
            Vector3 fromOriginToObject = target.position - center;
            fromOriginToObject *= radius / distance;
            target.position = center + fromOriginToObject;
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

        BoundTransform();
        drawLine();
    }

    private void drawLine()
    {
        Vector3[] points = new Vector3[] { Player1.transform.position + Vector3.up * offsetY, ankerPlayer1.position, ankerPlayer2.position, Player2.transform.position + Vector3.up * offsetY };
        _lr.positionCount = points.Length;
        _lr.SetPositions(points);
    }

    public bool IsOtherHolding(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return Player2.IsHolding;
        }
        else
        {
            return Player1.IsHolding;
        }
    }

    public void SetAnker(int playerNumber, Transform anker)
    {
        if (playerNumber == 1)
        {
            ankerPlayer1 = anker;
        }
        else
        {
            ankerPlayer2 = anker;
        }
    }
}