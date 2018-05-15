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

    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    public RopeController Rope;

    private float _radius;

    public float JumpForce;
    public float FallMutiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    public bool InCameraFocus;

    private float lookDir = 0;

    private bool isGrounded;
    private Rigidbody rb;
    private JoystickManager _joyManager;

    private void Start()
    {
        _radius = tower.localScale.x / 2;

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
        var h = -_joyManager.GetAxis(JoystickAxis.HORIZONTAL);
        var towerCenter = new Vector3(tower.position.x, transform.position.y, tower.position.z);
        var playerRadius = _radius + CharacterOffset;

        transform.RotateAround(tower.position, Vector3.up, h * Speed * Time.deltaTime);

        var desiredPos = (transform.position - tower.position).normalized * playerRadius + tower.position;

        if (Rope.CheckNewPosition(desiredPos, PlayerNumber))
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, Time.deltaTime * Speed);
        }

        // Coorect Look Dir
        if (h != 0)
            lookDir = (h > 0) ? 0 : 180;

        Vector3 tangentVector = Quaternion.Euler(0, lookDir, 0) * (transform.position - towerCenter);
        transform.rotation = Quaternion.LookRotation(tangentVector);

        // Check if inside Radius

        var distanceSqr = (transform.position - towerCenter).sqrMagnitude;
        float radiusSqr = _radius * _radius;

        if (distanceSqr != radiusSqr)
        {
            transform.position = towerCenter + (transform.position - towerCenter).normalized * playerRadius;
        }

        // Fix Cam
        if (InCameraFocus)
        {
            cam.transform.position = towerCenter + (transform.position - towerCenter).normalized * (playerRadius + CameraOffset);
            cam.transform.LookAt(transform);
        }
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