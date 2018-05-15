﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float Radius { get; private set; }

    public const float CharacterLayer = 5f;
    public const float CameraLayer = 10f;

    private void Start()
    {
        Radius = transform.localScale.x / 2;
    }
}