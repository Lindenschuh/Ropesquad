using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DProjection : MonoBehaviour
{
    // Use this for initialization
    public Camera cam;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        movement = Camera.main.transform.TransformDirection(movement);
        transform.Translate(movement);
    }
}