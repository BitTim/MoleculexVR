using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    public int id = -1;
    public GameObject collided = null;

    void OnTriggerEnter(Collider collider)
    {
        if(id == -1) { return; }

        collided = collider.gameObject;
        if (collided.tag != "Bond") { collided = null;  return; }

        transform.parent.gameObject.GetComponent<Atom>().bondCollided(id);
    }
}
