using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform Player;

    private bool _isJumping;

    private Vector3 peak;
    private Vector3 destination;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(new Vector3(0, -Input.GetAxis("Horizontal"), 0));

        transform.position = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z);

        if (!_isJumping)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _isJumping = true;
            }
        }
        else
        {
        }
    }
}