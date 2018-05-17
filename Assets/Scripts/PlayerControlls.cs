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
    public float UpForce = 1f;

    //public RopeController Rope;
    public RadialRope Rope;

    public float WalkSpeed = 2;
    public float Gravity = 12;
    public float JumpHeight = 1;

    public float PlayerLookOffset = 0;

    public bool IsAnchord;
    public Transform Anchor { get; private set; }
    public bool IsHolding { get; private set; }
    public bool CanHold;

    [Range(0, 1)]
    public float AirControlPercentage;

    private float lookDir = 0;
    public float velocityY;
    public float MinV;
    public float MaxV;
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
        AnchorCheck();
        Movement();
        Jumping();
    }

    private void AnchorCheck()
    {
        var offsetedPosition = transform.position + Vector3.up * Rope.offsetY;
        RaycastHit hit;
        Debug.DrawRay(offsetedPosition, (Rope.transform.position - offsetedPosition).normalized * (Rope.Radius * 2));
        if (Physics.Raycast(offsetedPosition, (Rope.transform.position - offsetedPosition).normalized, out hit, Rope.Radius * 2))
        {
            var hitObj = hit.collider.gameObject;
            if (hitObj.layer != _playerLayer)
            {
                if (!IsAnchord)
                {
                    Anchor = new GameObject().transform;
                    Anchor.position = hit.point;
                    Anchor.parent = hitObj.transform;
                }
                IsAnchord = true;
            }
            else
            {
                IsAnchord = false;
                if (Anchor != null)
                {
                    DestroyImmediate(Anchor.gameObject);
                    Anchor = null;
                }
            }
            Rope.SetAnker(PlayerNumber, Anchor);
        }
    }

    private void RopeInteraction()
    {
        // Einziehen
        if (_joyManager.CheckButton(JoystickButton.BUMPER_L, Input.GetButton))
        {
            Rope.changeRopeLength(-1f * Time.fixedDeltaTime);
        }
        // Seil lassen;
        if (_joyManager.CheckButton(JoystickButton.BUMPER_R, Input.GetButton))
        {
            Rope.changeRopeLength(1f * Time.fixedDeltaTime);
        }
    }

    private void Movement()
    {
        var h = -_joyManager.GetAxis(JoystickAxis.HORIZONTAL);
        var v = -_joyManager.GetAxis(JoystickAxis.VERTICAL);
        IsHolding = _joyManager.CheckButton(JoystickButton.B, Input.GetButton) && CanHold;

        if (!IsHolding)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, WalkSpeed, ref speedSmothvelocity, GetModifiedSmoothTime(SPEED_SMOTH_TIME));
            MovementManger.NextPosition(transform, h, currentSpeed, TowerObject, Tower.CharacterLayer, ref lookDir, PlayerLookOffset);

            if (!characterController.isGrounded && IsAnchord || !characterController.isGrounded && Rope.IsOtherHolding(PlayerNumber))
            {
                velocityY = Mathf.Clamp(velocityY - Gravity * Time.fixedDeltaTime, MinV, MaxV);
                velocityY = Mathf.Clamp(velocityY + v * UpForce * Time.fixedDeltaTime, MinV, MaxV);
            }
            else
            {
                velocityY -= Gravity * Time.fixedDeltaTime;
            }

            characterController.Move(new Vector3(0, velocityY, 0) * Time.fixedDeltaTime);

            if (characterController.isGrounded)
            {
                velocityY = 0;
            }
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
}