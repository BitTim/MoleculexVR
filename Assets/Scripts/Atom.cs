using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public int id = -1;
    public GameObject collided = null;

    public int type = -1;
    public GameObject bondPrefab;
    public List<int> numBonds = new List<int>();
    public List<Material> materials = new List<Material>();

    public List<GameObject> bonds = new List<GameObject>();
    public List<int> atomsAtBonds = new List<int>();
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if(type != -1) setType(type);
    }

    public void setType(int type)
    {
        this.type = type;

        if(type >= numBonds.Count || type >= materials.Count)
        {
            Debug.Log("[E] Atom: Index out of bounds");
            return;
        }

        if(type >= 0 && meshRenderer)
        {
            //Set correct material for nucleus
            meshRenderer.material = materials[type];
        }

        //Despawn all previous bonds
        foreach (Transform child in transform)
        {
            if (child == null) { continue; }

            Destroy(child.gameObject);
        }

        // Instanciate apropriate number of bonds
        for (int i = 0; i < numBonds[type]; i++)
        {
            // Spawn Bond
            GameObject bond = Instantiate(bondPrefab, gameObject.transform);
            bonds.Add(bond);
            bond.GetComponent<Bond>().id = i;

            // Initialize connection list with uninitialized IDs
            atomsAtBonds.Add(-1);

            // Apply Offset from center of Nucleus
            bond.transform.Translate(new Vector3(0, 0.035f, 0));
            
            if(i != 0)
            {
                // 120° rotation on Z-Axis
                bond.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), 120);
                // Multiple of 120° rotation on Y-Axis
                bond.transform.RotateAround(gameObject.transform.position, new Vector3(0, 1, 0), 120 * (i - 1));
            }
        }
    }

    public void bondCollided(int bondID)
    {
        List<int> ids = new List<int>();
        ids.Add(id);
        ids.Add(bondID);

        if (bonds[bondID].GetComponent<Bond>().collided == null) return;
        collided = bonds[bondID].GetComponent<Bond>().collided.transform.parent.gameObject;
        transform.parent.GetComponent<Molecule>().atomCollided(ids);

        bonds[bondID].GetComponent<Bond>().collided = null;
    }
}
