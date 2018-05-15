using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joysticks;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerControlls : MonoBehaviour
{
    private static float SPEED_SMOTH_TIME = .1f;

    // Use this for initialization
    public Tower TowerObject;

    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    public RopeController Rope;

    public float WalkSpeed = 2;
    public float Gravity = 12;
    public float JumpHeight = 1;

    [Range(0, 1)]
    public float AirControlPercentage;

    private float lookDir = 0;
    private float velocityY;
    private float currentSpeed;
    private JoystickManager _joyManager;
    private CharacterController characterController;

    private float speedSmothvelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _joyManager = new JoystickManager(PlayerNumber);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RopeInteraction();
        Movement();
        Jumping();
    }

    private void RopeInteraction()
    {
        // Einziehen
        if (_joyManager.CheckButton(JoystickButton.BUMPER_L, Input.GetButton))
        {
            Rope.UpdateWinch(Rope.RopeLength - Rope.WinchSpeed * Time.fixedDeltaTime, TowerObject.transform.position, TowerObject.Radius, PlayerNumber);
        }
        // Seil lassen;
        if (_joyManager.CheckButton(JoystickButton.BUMPER_R, Input.GetButton))
        {
            Rope.UpdateWinch(Rope.RopeLength + Rope.WinchSpeed * Time.fixedDeltaTime, TowerObject.transform.position, TowerObject.Radius, PlayerNumber);
        }
    }

    private void Movement()
    {
        var h = -_joyManager.GetAxis(JoystickAxis.HORIZONTAL);

        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, WalkSpeed, ref speedSmothvelocity, GetModifiedSmoothTime(SPEED_SMOTH_TIME));
        MovementManger.NextPosition(transform, h, currentSpeed, TowerObject, Tower.CharacterLayer, ref lookDir);

        velocityY -= Gravity * Time.fixedDeltaTime;

        characterController.Move(new Vector3(0, velocityY, 0) * Time.fixedDeltaTime);

        if (!Rope.CheckNewPosition(transform.position, PlayerNumber))
        {
            if (velocityY <= 0)
            {
                transform.position = new Vector3(oldPosition.x, transform.position.y, oldPosition.z);
            }
            else
            {
                transform.position = oldPosition;
                velocityY = 0;
            }

            transform.rotation = oldRotation;
        }

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }
    }

    private void Jumping()
    {
        if (_joyManager.CheckButton(JoystickButton.A, Input.GetButtonDown) && characterController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2 * Gravity * JumpHeight);
            velocityY = jumpVelocity;
        }
    }

    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
            return smoothTime;

        if (AirControlPercentage == 0)
            return float.MaxValue;

        return smoothTime / AirControlPercentage;
    }
}