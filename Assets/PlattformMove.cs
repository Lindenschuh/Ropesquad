using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public enum MoveDir
    {
        HORIZONTAL,
        VERTICAL,
        NONE
    }

    public class PlattformMove : MonoBehaviour
    {
        [Range(.2f, 2f)]
        public float MoveDistance = 1;

        public Tower Tower;

        public float MovementSpeed;

        public MoveDir Direction;

        private float lookDir;

        private void Start()
        {
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            switch (Direction)
            {
                case MoveDir.HORIZONTAL:
                    MovementManger.NextPlattformPosition(transform, Mathf.Sin(Time.time / MoveDistance), MovementSpeed, Tower, Tower.CharacterLayer);
                    break;

                case MoveDir.VERTICAL:
                    transform.Translate(Vector3.up * Mathf.Sin(Time.time / MoveDistance) * MovementSpeed * Time.fixedDeltaTime);
                    MovementManger.SnapToGrid(transform, Tower, Tower.CharacterLayer);
                    break;

                case MoveDir.NONE:
                    MovementManger.SnapToGrid(transform, Tower, Tower.CharacterLayer);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9)
            {
                other.transform.parent = gameObject.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 9)
            {
                other.transform.parent = null;
            }
        }
    }
}