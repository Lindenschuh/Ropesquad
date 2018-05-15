using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float Radius { get; private set; }

    private void Start()
    {
        Radius = transform.localScale.x / 2;
    }
}