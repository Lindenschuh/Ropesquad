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

    public GameObject AnkerPrefab;

    private GameObject ankerPlayer1;
    private GameObject ankerPlayer2;

    private LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();

        transform.position = (Player1.transform.position -
            (Player1.transform.position - Player2.transform.position) / 2)
            + Vector3.up * offsetY;
    }

    public void BoundTransform()
    {
        if (ankerPlayer1 == null)
            ankerPlayer1 = Instantiate(AnkerPrefab, transform.position, Quaternion.identity);
        if (ankerPlayer2 == null)
            ankerPlayer2 = Instantiate(AnkerPrefab, transform.position, Quaternion.identity);

        Player1.LookAtPoints(Player2, ankerPlayer2.transform, ankerPlayer1.transform);
        Player2.LookAtPoints(Player1, ankerPlayer1.transform, ankerPlayer2.transform);

        Vector3 positionP1 = Player1.transform.position + Vector3.up * offsetY;
        Vector3 positionP2 = Player2.transform.position + Vector3.up * offsetY;

        avdPl1 = (positionP1 - ankerPlayer1.transform.position).magnitude;
        subDist = (ankerPlayer1.transform.position - ankerPlayer2.transform.position).magnitude;
        avdPl2 = (positionP2 - ankerPlayer2.transform.position).magnitude;

        float fullDistance = Radius * 2;
        float remainingDistance = fullDistance - subDist;
        float availableDistance = remainingDistance - avdPl1 - avdPl2;

        float radiusP1 = (avdPl1 + availableDistance / 2) / 2;
        float radiusP2 = (avdPl2 + availableDistance / 2) / 2;

        Vector3 centerPositionP1 = positionP1 - ((positionP1 - ankerPlayer1.transform.position) / 2);
        Vector3 centerPositionP2 = positionP2 - ((positionP2 - ankerPlayer2.transform.position) / 2);

        ApplyBoundries(Player1.transform, radiusP1, centerPositionP1);
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

    public void ChangeRopeLength(float deltaLength)
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
        DrawLine();
    }

    private void DrawLine()
    {
        Vector3[] points = new Vector3[] { Player1.transform.position + Vector3.up * offsetY, ankerPlayer1.transform.position, ankerPlayer2.transform.position, Player2.transform.position + Vector3.up * offsetY };
        _lr.positionCount = points.Length;
        _lr.SetPositions(points);
    }
}