using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHangleHandler : MonoBehaviour
{
    public Tower GameTower;

    private int _playerLayer;

    void Start()
    {
        MovementManger.SnapToGrid(transform, GameTower, Tower.CharacterLayer);
        _playerLayer = LayerMask.NameToLayer("Player");
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