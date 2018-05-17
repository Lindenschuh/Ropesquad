using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndipendentRope : MonoBehaviour
{
    public Transform SegmentPrefab;

    public Transform head;
    public Transform tail;

    public float Gravity = -1f;
    public int Points = 5;

    public float Distance = 1;
    public RopePoint[] _ropePoints;
    private RopeStick[] _ropeSticks;

    private void Start()
    {
        _ropePoints = new RopePoint[Points + 2];
        _ropeSticks = new RopeStick[Points + 1];
        _ropePoints[0] = new RopePoint(head);
        _ropePoints[Points + 1] = new RopePoint(tail);

        CreateRope();

        updatePoints();
        for (int i = 0; i < 100; i++)
        {
            updateSticks();
            constrainPoints();
        }
    }

    private void CreateRope()
    {
        for (int i = 1; i <= Points + 1; i++)
        {
            if (i != Points + 1)
            {
                var segment = Instantiate(SegmentPrefab);
                segment.transform.position = new Vector3(head.transform.position.x - i * 0.5f, head.transform.position.y, head.transform.position.z);
                _ropePoints[i] = new RopePoint(segment);
            }

            _ropeSticks[i - 1] = new RopeStick() { p0 = _ropePoints[i - 1], p1 = _ropePoints[i], length = Distance };
        }
    }

    private void LateUpdate()
    {
        updatePoints();

        updateSticks();
        constrainPoints();
    }

    private void updatePoints()
    {
        foreach (RopePoint rp in _ropePoints)
        {
            float vx = rp.Point.position.x - rp.oldX;
            float vy = rp.Point.position.y - rp.oldY;
            float vz = rp.Point.position.z - rp.oldZ;
            rp.oldX = rp.Point.position.x;
            rp.oldY = rp.Point.position.y;
            rp.oldZ = rp.Point.position.z;
            rp.Point.position = new Vector3(rp.Point.position.x + vx * Time.fixedDeltaTime, rp.Point.position.y + vy * Time.fixedDeltaTime, rp.Point.position.z + vz * Time.fixedDeltaTime);
        }
    }

    private void updateSticks()
    {
        foreach (RopeStick rs in _ropeSticks)
        {
            rs.length = Distance;
            Vector3 deltaV = rs.p1.Point.position - rs.p0.Point.position;
            float distance = deltaV.magnitude;
            float difference = rs.length - distance;

            float precent = difference / distance / 2;
            Vector3 offset = deltaV * precent;
            if (rs.p0.Point != head && rs.p0.Point != tail)
                rs.p0.Point.position -= offset;
            if (rs.p1.Point != head && rs.p1.Point != tail)
                rs.p1.Point.position += offset;
        }
    }

    private void constrainPoints()
    {
        foreach (RopePoint rp in _ropePoints)
        {
        }
    }
}

public class RopePoint
{
    public Transform Point;
    public float oldX;
    public float oldY;
    public float oldZ;

    public RopePoint(Transform ground)
    {
        Point = ground;
        oldX = ground.transform.position.x;
        oldY = ground.transform.position.y;
        oldZ = ground.transform.position.z;
    }
}

public class RopeStick
{
    public RopePoint p0;
    public RopePoint p1;
    public float length;
}