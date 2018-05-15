using UnityEngine;
using Joysticks;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlls : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public float FallMutiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    public byte PlayerNumber;

    private bool isGrounded;
    private Rigidbody rb;
    private JoystickManager joyManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joyManager = new JoystickManager(PlayerNumber);
    }

    private void FixedUpdate()
    {
        var h = joyManager.GetAxis(JoystickAxis.HORIZONTAL);

        transform.position += new Vector3(h * Speed, 0, 0) * Time.deltaTime;

        if (joyManager.CheckButton(JoystickButton.A, Input.GetButtonDown) && isGrounded)
        {
            rb.velocity += Vector3.up * JumpForce;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (FallMutiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !joyManager.CheckButton(JoystickButton.A, Input.GetButton))
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