using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool useSimple = false;
    public Transform target;
    public Vector3 offset;

    void FixedUpdate()
    {
        if(useSimple)
        {
            transform.position = new Vector3(target.position.x, 0, target.position.z) + offset;
            return;
        }

        transform.position = target.position + Vector3.up * offset.y
            + Vector3.ProjectOnPlane(target.right, Vector3.up).normalized * offset.x
            + Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * offset.z;

        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}
