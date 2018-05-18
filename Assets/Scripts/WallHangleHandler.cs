using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHangleHandler : MonoBehaviour
{
    public Tower GameTower;

    private int _playerLayer;

    private void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
    }

    private void FixedUpdate()
    {
        MovementManger.SnapToGrid(transform, GameTower, Tower.CharacterLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            other.GetComponent<PlayerControlls>().CanHold = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            other.GetComponent<PlayerControlls>().CanHold = false;
        }
    }
}