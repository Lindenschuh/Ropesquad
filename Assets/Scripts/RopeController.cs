using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Transform ConnectionOne;
    public Transform ConnectionTwo;

    private LineRenderer lineRenderer;

    public List<Vector3> RopeSections;

    [Range(1f, 20f)]
    public float RopeLength = 1f;

    public float MinRopeLength = 1f;
    public float MaxRopeLength = 20f;

    [Range(0f, 0.05f)]
    public float stretch = 0.01f;

    public float RopeWidth = .2f;

    public float WinchSpeed = .2f;

    private float loadMass = 100f;

    private SpringJoint springJoint;

    private void Start()
    {
        springJoint = ConnectionOne.GetComponent<SpringJoint>();

        lineRenderer = GetComponent<LineRenderer>();

        UpdateSpring();
    }

    private void Update()
    {
        DisplayRope();
    }

    private void UpdateSpring()
    {
        float density = 7750f;

        float radius = 0.02f;

        float volume = Mathf.PI * radius * radius * RopeLength;

        float ropeMass = volume * density;

        ropeMass += loadMass;

        float ropeForce = ropeMass * Physics.gravity.y;

        float kRope = ropeForce / stretch;

        springJoint.spring = kRope * 1f;
        springJoint.damper = kRope * 0.8f;

        springJoint.maxDistance = RopeLength;
    }

    public void UpdateWinch(float newLength)
    {
        RopeLength = Mathf.Clamp(newLength, MinRopeLength, MaxRopeLength);

        UpdateSpring();
    }

    public bool IsOnMaxLength()
    {
        return (ConnectionOne.position - ConnectionTwo.position).magnitude >= RopeLength;
    }

    public bool CheckNewPosition(Vector3 newPos, byte player)
    {
        if (player == 1)
        {
            return (newPos - ConnectionTwo.position).magnitude < RopeLength;
        }
        else
        {
            return (newPos - ConnectionOne.position).magnitude < RopeLength;
        }
    }

    private void DisplayRope()
    {
        lineRenderer.startWidth = RopeWidth;
        lineRenderer.endWidth = RopeWidth;

        Vector3 A = ConnectionOne.position;
        Vector3 D = ConnectionTwo.position;

        Vector3 B = A + Vector3.zero * .5f * (-(A - D).magnitude * 0.1f);

        Vector3 C = D + Vector3.zero * .5f * ((A - D).magnitude * .5f);

        BezierCurve.GetBezierCurve(A, B, C, D, out RopeSections);

        lineRenderer.positionCount = RopeSections.Count;

        lineRenderer.SetPositions(RopeSections.ToArray());
    }
}