using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBehaivor : MonoBehaviour
{
    public Rigidbody MasterRB;
    public Rigidbody SlaveRB;

    public float MaxSize = 15;
    public float MinSize = 1;

    private ConfigurableJoint _joint;

    private void Start()
    {
        _joint = MasterRB.GetComponent<ConfigurableJoint>();
    }

    public void ChangeRope(float amount)
    {
        SoftJointLimit sLimit = new SoftJointLimit();
        sLimit.limit = _joint.linearLimit.limit - amount;
        _joint.linearLimit = sLimit;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = MasterRB.position - (MasterRB.position - SlaveRB.position) / 2;
    }
}