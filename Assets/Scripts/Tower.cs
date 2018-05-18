﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tower : MonoBehaviour
{
    public float Radius { get; private set; }

    public const float CharacterLayer = 2f;

    private void Awake()
    {
        Radius = transform.localScale.x / 2;
    }
}