using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pouch : MonoBehaviour
{
    public int id = -1;
    public GameObject moleculeScene;

    private GameObject newsetMolecule;
    private bool grabbed = false;

    void Start()
    {
        if(!newsetMolecule) spawnMolecule();
    }

    public void spawnMolecule()
    {
        if (!moleculeScene || id == -1) return;
        GameObject molecule = Instantiate(moleculeScene, transform.position, Quaternion.identity);

        molecule.GetComponent<Molecule>().addAtom(-1, -1, id);
        molecule.layer = 8;

        newsetMolecule = molecule;
    }

    public void grabMolecule()
    {
        grabbed = true;
        newsetMolecule.GetComponent<Molecule>().updateLayer(9);
    }

    public void abandonMolecule()
    {
        if(!grabbed) return;
        newsetMolecule.GetComponent<Molecule>().updateLayer(0);
        grabbed = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if(id != -1) return;
        Destroy(collider.gameObject);
    }
}
