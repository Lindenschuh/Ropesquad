using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joysticks;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerControlls : MonoBehaviour
{
    private static float SPEED_SMOTH_TIME = .1f;

    // Use this for initialization
    public Tower TowerObject;

    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    //public RopeController Rope;
    public RadialRope Rope;

    public float WalkSpeed = 2;
    public float Gravity = 12;
    public float JumpHeight = 1;

    public float PlayerLookOffset = 0;

    public bool IsAnchord;
    public Transform Anchor { get; private set; }

    public LayerMask Boundaries;

    [Range(0, 1)]
    public float AirControlPercentage;

    private float lookDir = 0;
    public float velocityY;
    private float currentSpeed;
    private JoystickManager _joyManager;
    private CharacterController characterController;
    private Animator _animator;

    private float speedSmothvelocity;

    private int _playerLayer;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _joyManager = new JoystickManager(PlayerNumber);
        _animator = GetComponent<Animator>();
        _playerLayer = LayerMask.NameToLayer("Player");
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
            Rope.ChangeRopeLength(-1f * Time.fixedDeltaTime);
        }
        // Seil lassen;
        if (_joyManager.CheckButton(JoystickButton.BUMPER_R, Input.GetButton))
        {
            Rope.ChangeRopeLength(1f * Time.fixedDeltaTime);
        }
    }

    private void Movement()
    {
        var h = -_joyManager.GetAxis(JoystickAxis.HORIZONTAL);

        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, WalkSpeed, ref speedSmothvelocity, GetModifiedSmoothTime(SPEED_SMOTH_TIME));
        MovementManger.NextPosition(transform, h, currentSpeed, TowerObject, Tower.CharacterLayer, ref lookDir, PlayerLookOffset);

        velocityY -= Gravity * Time.fixedDeltaTime;

        characterController.Move(new Vector3(0, velocityY, 0) * Time.fixedDeltaTime);

        if (characterController.isGrounded)
        {
            velocityY = 0;
        }

        UpdateAnimator(Mathf.Abs(h), characterController.isGrounded);
    }

    private void UpdateAnimator(float input, bool grounded)
    {
        _animator.SetFloat("WalkSpeed", input, 0.1f, Time.fixedDeltaTime);
        _animator.SetBool("OnGround", grounded);
        if (!grounded)
        {
            _animator.SetFloat("Jump", velocityY);
        }

        if (velocityY == 0)
        {
            _animator.SetFloat("Jump", velocityY);
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

    public void LookAtPoints(PlayerControlls otherPlayer, Transform otherAnker, Transform myAnker)
    {
        var correctedPosition = transform.position + Vector3.up * Rope.offsetY;
        var correctedOtherPosition = otherPlayer.transform.position + Vector3.up * Rope.offsetY;

        RaycastHit otherPlayerHit;
        Ray otherPlayerRay = new Ray(correctedPosition, (correctedOtherPosition - correctedPosition).normalized);
        Debug.DrawRay(otherPlayerRay.origin, otherPlayerRay.direction, Color.blue, Rope.Radius);
        bool otherPlayerHitted = false;
        if (Physics.Raycast(otherPlayerRay, out otherPlayerHit, Rope.Radius, Boundaries))
        {
            otherPlayerHitted = otherPlayerHit.transform.position + Vector3.up * Rope.offsetY == correctedOtherPosition;
        }

        RaycastHit otherAnkerHit;
        Ray otherAnkerRay = new Ray(correctedPosition, (otherAnker.position - correctedPosition).normalized);
        Debug.DrawRay(otherAnkerRay.origin, otherAnkerRay.direction, Color.yellow, Rope.Radius);
        bool otherAnkerHitted = false;
        if (Physics.Raycast(otherAnkerRay, out otherAnkerHit, Rope.Radius))
        {
            otherAnkerHitted = otherAnkerHit.transform.position == otherAnker.position;
        }

        RaycastHit myAnkerHit;
        Ray myAnkerRay = new Ray(correctedPosition, (myAnker.position - correctedPosition).normalized);
        Debug.DrawRay(myAnkerRay.origin, myAnkerRay.direction, Color.magenta, Rope.Radius);
        bool myAnkerHitted = false;
        if (Physics.Raycast(myAnkerRay, out myAnkerHit, Rope.Radius))
        {
            myAnkerHitted = myAnkerHit.transform.position == myAnker.position;
        }

        Debug.Log($"{PlayerNumber}: Other Player: {otherPlayerHitted}, OtherAnker: {otherAnkerHitted}, MyAnker: {myAnkerHitted} ");

        if (otherPlayerHitted && otherAnkerHitted && myAnkerHitted)
        {
            myAnker.position = Rope.transform.position;
        }
        else if (otherAnkerHitted && myAnkerHitted)
        {
            myAnker.position = otherAnker.position;
        }
        else if (myAnkerHitted)
        {
            myAnker.position = myAnkerHit.point;
        }
    }
}