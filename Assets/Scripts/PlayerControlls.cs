using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joysticks;
using System;

public class PlayerControlls : MonoBehaviour
{
    // Use this for initialization
    public Camera cam;

    public Tower tower;

    public float Speed;

    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    public RopeController Rope;

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
        rb = GetComponent<Rigidbody>();
        _joyManager = new JoystickManager(PlayerNumber);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RopeInteraction();
        Movement();
        Jumping();
        CheckPosition();
    }

    private void RopeInteraction()
    {
        // Einziehen
        if (_joyManager.CheckButton(JoystickButton.BUMPER_L, Input.GetButton))
        {
            Rope.UpdateWinch(Rope.RopeLength - Rope.WinchSpeed * Time.fixedDeltaTime, tower.transform.position, tower.Radius, PlayerNumber);
        }
        // Seil lassen;
        if (_joyManager.CheckButton(JoystickButton.BUMPER_R, Input.GetButton))
        {
            Rope.UpdateWinch(Rope.RopeLength + Rope.WinchSpeed * Time.fixedDeltaTime, tower.transform.position, tower.Radius, PlayerNumber);
        }
    }

    private void Movement()
    {
        var h = -_joyManager.GetAxis(JoystickAxis.HORIZONTAL);
        var towerCenter = new Vector3(tower.transform.position.x, transform.position.y, tower.transform.position.z);
        var playerRadius = tower.Radius + CharacterOffset;

        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        MovementManger.NextPosition(transform, h, Speed, tower, CharacterOffset, ref lookDir);

        if (!Rope.CheckNewPosition(transform.position, PlayerNumber))
        {
            transform.position = oldPosition;
            transform.rotation = oldRotation;
        }

        // Fix Cam
        if (InCameraFocus)
        {
            var ropePosition = Rope.GetComponent<LineRenderer>().bounds.center;

            cam.transform.position = towerCenter + (ropePosition - towerCenter).normalized * (playerRadius + CameraOffset);
            cam.transform.LookAt(ropePosition);
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