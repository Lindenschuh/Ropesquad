using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DProjection : MonoBehaviour
{
    // Use this for initialization
    public Camera cam;

    public Transform tower;

    private float RotateSpeed = 1f;
    public float CharacterOffset = 5f;
    public float CameraOffset = 10;

    private Vector2 _centre;
    private float _angle;
    private float _timer;
    private float _radius;

    private void Start()
    {
        _centre = transform.position;
        _radius = tower.localScale.x / 2 + CharacterOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        cam.transform.LookAt(new Vector3(tower.position.x, transform.position.y, tower.position.z));
        _timer += -Input.GetAxis("Horizontal") * Time.deltaTime * 0.1f;
        var angle = _timer;
        this.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * _radius), transform.position.y, ((_centre.y + Mathf.Cos(angle) * _radius)));
        cam.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * (_radius + CameraOffset)), transform.position.y, ((_centre.y + Mathf.Cos(angle) * (_radius + CameraOffset))));
    }
}