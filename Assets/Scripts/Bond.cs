using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    public int id = -1;
    public GameObject collided = null;

    void OnCollisionEnter(Collision collision)
    {
        if(id == -1) { return; }

        collided = collision.collider.gameObject;
        if (collided.tag != "Bond") { collided = null;  return; }

        transform.parent.GetComponent<Atom>().bondCollided(id);
    }
}
