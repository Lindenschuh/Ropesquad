using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joysticks;
using System;

public class PlayerControlls : MonoBehaviour
{
    // Use this for initialization
    public Camera cam;

    public Transform tower;

    public float Speed;

    private float RotateSpeed = 1f;
    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    public RopeController Rope;

    public float StartAngle = 5;

    private Vector2 _centre;
    private Vector2 _camCentre;
    private float _angle;
    private float _timer;
    private float _radius;

    public float JumpForce;
    public float FallMutiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    public bool InCameraFocus;

    public float LookDir = 45f;

    private bool isGrounded;
    private Rigidbody rb;
    private JoystickManager _joyManager;

    private void Start()
    {
        _radius = tower.localScale.x / 2 + CharacterOffset;

        var x = _radius * Mathf.Sin(StartAngle);
        var z = _radius * Mathf.Cos(StartAngle);

        transform.position = new Vector3(x, transform.position.y, z);

        if (InCameraFocus)
        {
            x = (_radius + CameraOffset) * Mathf.Sin(StartAngle);
            z = (_radius + CameraOffset) * Mathf.Cos(StartAngle);

            cam.transform.position = new Vector3(x, transform.position.y, z);
            _camCentre = new Vector2(transform.position.x, transform.position.z);
        }

        _centre = new Vector2(transform.position.x, transform.position.z);
        rb = GetComponent<Rigidbody>();
        _joyManager = new JoystickManager(PlayerNumber);
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Jumping();
        RopeInteraction();
        CheckPosition();
    }

    private void RopeInteraction()
    {
        // Einziehen
        if (_joyManager.CheckButton(JoystickButton.BUMPER_L, Input.GetButton) && Rope.RopeLength > Rope.MinRopeLength)
        {
            Rope.UpdateWinch(Rope.RopeLength - Rope.WinchSpeed * Time.deltaTime);
        }
        // Seil lassen;
        if (_joyManager.CheckButton(JoystickButton.BUMPER_L, Input.GetButton) && Rope.RopeLength < Rope.MaxRopeLength)
        {
            Rope.UpdateWinch(Rope.RopeLength + Rope.WinchSpeed * Time.deltaTime);
        }
    }

    private void Movement()
    {
        var h = _joyManager.GetAxis(JoystickAxis.HORIZONTAL);

        transform.RotateAround(tower.position, Vector3.up, h * Speed * Time.deltaTime);

        var desiredPos = (transform.position - tower.position).normalized * _radius + tower.position;

        if (Rope.CheckNewPosition(desiredPos, PlayerNumber))
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, Time.deltaTime * Speed);
        }

        // Coorect Look Dir
        var lookDir = (h > 0) ? 0 : 180;
        Vector3 tangentVector = Quaternion.Euler(0, lookDir, 0) * (transform.position - new Vector3(tower.position.x, transform.position.y, tower.position.z));
        transform.rotation = Quaternion.LookRotation(tangentVector);

        // Check if inside Radius
        var towerCenter = new Vector3(tower.position.x, transform.position.y, tower.position.z);
        var distanceSqr = (transform.position - towerCenter).sqrMagnitude;
        float radiusSqr = _radius * _radius;

        if (distanceSqr != radiusSqr)
        {
            transform.position = towerCenter + (transform.position - towerCenter).normalized * _radius;
        }

        //var tmpTimer = _timer - _joyManager.GetAxis(JoystickAxis.HORIZONTAL) * Time.deltaTime * Speed;
        //var angle = tmpTimer;
        //var newPos = new Vector3((_centre.x + Mathf.Sin(angle) * _radius), transform.position.y, ((_centre.y + Mathf.Cos(angle) * _radius)));
        ////if (Rope.CheckNewPosition(newPos, transform))
        ////{
        //_timer = tmpTimer;
        //transform.position = newPos;

        //if (InCameraFocus)
        //{
        //    cam.transform.LookAt(new Vector3(tower.position.x, transform.position.y, tower.position.z));
        //    cam.transform.position = new Vector3((_camCentre.x + Mathf.Sin(angle) * (_radius + CameraOffset)), transform.position.y, ((_camCentre.y + Mathf.Cos(angle) * (_radius + CameraOffset))));
        //}
    }

    private void CheckPosition()
    {
    }

    private void Jumping()
    {
        if (_joyManager.CheckButton(JoystickButton.A, Input.GetButtonDown) && isGrounded)
        {
            rb.velocity += Vector3.up * JumpForce;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (FallMutiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !_joyManager.CheckButton(JoystickButton.A, Input.GetButton))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}