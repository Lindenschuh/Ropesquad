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

    public float JumpForce;
    public float FallMutiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    private bool isGrounded;
    private Rigidbody rb;

    private void Start()
    {
        _centre = transform.position;
        _radius = tower.localScale.x / 2 + CharacterOffset;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        cam.transform.LookAt(new Vector3(tower.position.x, transform.position.y, tower.position.z));
        _timer += -Input.GetAxis("Horizontal") * Time.deltaTime * 0.5f;
        var angle = _timer;
        this.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * _radius), transform.position.y, ((_centre.y + Mathf.Cos(angle) * _radius)));
        cam.transform.position = new Vector3((_centre.x + Mathf.Sin(angle) * (_radius + CameraOffset)), transform.position.y, ((_centre.y + Mathf.Cos(angle) * (_radius + CameraOffset))));

        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            rb.velocity += Vector3.up * JumpForce;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (FallMutiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Fire1"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}