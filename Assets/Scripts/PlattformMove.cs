using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public enum MoveDir
    {
        HORIZONTAL,
        VERTICAL,
        FREE,
        NONE
    }

    public class PlattformMove : MonoBehaviour
    {
        public const float TARGET_DISTANCE = 0.1f;

        public Tower GameTower;

        public float MovementSpeed = 3;

        public MoveDir Direction;

        public Transform PointA;
        public Transform PointB;

        public bool InverseStart;

        private Vector3 velocity;

        private Transform target;

        private float approcheRadius;
        private int playerLayer;

        private void Start()
        {
            MovementManger.SnapToGrid(transform, GameTower, Tower.CharacterLayer);
            playerLayer = LayerMask.NameToLayer("Player");

            if (Direction != MoveDir.NONE)
            {
                if (PointA != null && PointB != null)
                {
                    target = (InverseStart) ? PointB : PointA;
                    approcheRadius = transform.localScale.x / 2;

                    switch (Direction)
                    {
                        case MoveDir.HORIZONTAL:
                            PointA.position = new Vector3(PointA.position.x, transform.position.y, PointA.position.z);
                            PointB.position = new Vector3(PointB.position.x, transform.position.y, PointB.position.z);

                            MovementManger.SnapToGrid(PointA, GameTower, Tower.CharacterLayer);
                            MovementManger.SnapToGrid(PointB, GameTower, Tower.CharacterLayer);
                            break;

                        case MoveDir.VERTICAL:
                            PointA.position = new Vector3(transform.position.x, PointA.position.y, transform.position.z);
                            PointB.position = new Vector3(transform.position.x, PointB.position.y, transform.position.z);

                            MovementManger.SnapToGrid(PointA, GameTower, Tower.CharacterLayer);
                            MovementManger.SnapToGrid(PointB, GameTower, Tower.CharacterLayer);
                            break;

                        case MoveDir.FREE:
                            MovementManger.SnapToGrid(PointA, GameTower, Tower.CharacterLayer);
                            MovementManger.SnapToGrid(PointB, GameTower, Tower.CharacterLayer);
                            break;
                    }
                }
                else
                {
                    Debug.LogError($"Please add the Target Points A & B to the {this.GetType().ToString()}");
                }
            }
        }

        private void FixedUpdate()
        {
            if (Direction != MoveDir.NONE)
                SteerToTarget();
        }

        private void SteerToTarget()
        {
            var desiredVelocity = target.position - transform.position;
            var distance = desiredVelocity.magnitude;

            if (distance < TARGET_DISTANCE)
            {
                SwitchTarget();
                return;
            }

            if (distance < approcheRadius)
            {
                desiredVelocity = desiredVelocity.normalized * MovementSpeed * (distance / approcheRadius);
            }
            else
            {
                desiredVelocity = desiredVelocity.normalized * MovementSpeed;
            }

            transform.position += desiredVelocity * Time.fixedDeltaTime;

            MovementManger.SnapToGrid(transform, GameTower, Tower.CharacterLayer);
        }

        private void SwitchTarget() => target = (target == PointA) ? PointB : PointA;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == playerLayer)
            {
                other.transform.parent = gameObject.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == playerLayer)
            {
                other.transform.parent = null;
            }
        }
    }
}