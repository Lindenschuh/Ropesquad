using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joysticks;

public class PlayerControlls : MonoBehaviour
{
    // Use this for initialization
    public Camera cam;

    public Transform tower;

    [Range(0f, 1f)]
    public float Speed;

    private float RotateSpeed = 1f;
    public float CharacterOffset = 5f;
    public float CameraOffset = 10;
    public byte PlayerNumber;

    private Vector2 _centre;
    private float _angle;
    private float _timer;
    private float _radius;

    public float JumpForce;
    public float FallMutiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    public bool InCameraFocus;

    private bool isGrounded;
    private Rigidbody rb;
    private JoystickManager _joyManager;

    private void Start()
    {
        _centre = transform.position;
        _radius = tower.localScale.x / 2 + CharacterOffset;
        rb = GetComponent<Rigidbody>();
        _joyManager = new JoystickManager(PlayerNumber);
    }

    // Update is called once per frame
    private void Update()
    {
        cam.transform.LookAt(new Vector3(tower.position.x, transform.position.y, tower.position.z));
        _timer += -_joyManager.GetAxis(JoystickAxis.HORIZONTAL) * Time.deltaTime * Speed;
        var angle = _timer;
        this.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * _radius), transform.position.y, ((_centre.y + Mathf.Cos(angle) * _radius)));
        if (InCameraFocus)
            cam.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * (_radius + CameraOffset)), transform.position.y, ((_centre.y + Mathf.Cos(angle) * (_radius + CameraOffset))));

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