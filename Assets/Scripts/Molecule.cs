using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Molecule : MonoBehaviour
{
    public int id = -1;

    public bool spawnRoot = false;
    public int rootID = -1;

    public GameObject atomPrefab;
    public List<GameObject> atoms = new List<GameObject>();

    public GameObject idManager;
    private int nextAtomID = 0;

    void Start()
    {
        if(!idManager) { return; }
        
        id = idManager.GetComponent<MoleculeIDManager>().getNextID();
        if(id == -1) return;

        if(spawnRoot && rootID != -1) addAtom(-1, -1, rootID);
    }

    public void addAtom(int atomID, int bondID, int type)
    {
        if(type == -1) { return; }

        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion rot = Quaternion.identity;

        if(atomID != -1 && bondID != -1)
        {
            pos = atoms[atomID].transform.position - transform.position;
            pos.y += 0.0875f;
            
            rot = atoms[atomID].GetComponent<Atom>().bonds[bondID].transform.rotation;
            rot = rot * Quaternion.Euler(0, 0, 180);
        }
        else
        {
            if (atoms.Count != 0 || nextAtomID != 0) { return; }
        }

        GameObject atom;

        if(!atomPrefab) return;
        atom = Instantiate(atomPrefab, transform);

        atom.GetComponent<Atom>().transform.Translate(pos);
        atom.GetComponent<Atom>().transform.Rotate(rot.eulerAngles);

        if(!atom) return;

        atoms.Add(atom);
        atom.GetComponent<Atom>().setType(type);
        atom.GetComponent<Atom>().id = nextAtomID;

        atoms[atomID].GetComponent<Atom>().atomsAtBonds[bondID] = nextAtomID;
        nextAtomID++;

        updateBoxCollider();
    }

    public void addMolecule(int atomID, int bondID, GameObject molecule)
    {
        List<GameObject> atoms = molecule.GetComponent<Molecule>().atoms;
        int id = molecule.GetComponent<Molecule>().id;

        if(this.atoms.Count < atoms.Count) { return; }
        if(this.atoms.Count == atoms.Count && this.id > id) { return; }

        Vector3 tragetPos = this.atoms[atomID].transform.position - transform.position + new Vector3(0, 0.0875f, 0);
        Quaternion targetRot = this.atoms[atomID].GetComponent<Atom>().bonds[bondID].transform.rotation;

        for (int i = 0; i < atoms.Count; i++)
        {
            int oldID = atoms[i].GetComponent<Atom>().id;
            int newID = nextAtomID++;

            atoms[i].GetComponent<Atom>().id = newID;

            // Change Atom IDs
            List<int> atomsAtBonds = atoms[i].GetComponent<Atom>().atomsAtBonds;
            for(int j = 0; j < atomsAtBonds.Count; j++)
            {
                List<int> changedAtomsAtBonds = atoms[atomsAtBonds[j]].GetComponent<Atom>().atomsAtBonds;
                for (int k = 0; k < atomsAtBonds.Count; k++)
                {
                    if(changedAtomsAtBonds[k] != oldID) { continue; }
                    changedAtomsAtBonds[k] = newID;
                }
            }

            this.atoms.Add(atoms[i]);
        }
    }

    void updateBoxCollider()
    {
        var b = new Bounds(transform.position, Vector3.zero);
        foreach(GameObject a in atoms)
        {
            b.Encapsulate(a.GetComponent<Renderer>().bounds);
        }

        BoxCollider bc = gameObject.GetComponent<BoxCollider>();
        bc.size = b.size;
        bc.center = b.center - transform.position;
    }

    public void updateLayer(int layer)
    {
        gameObject.layer = layer;
    }

    public void destroy()
    {
        Destroy(gameObject.GetComponent<XRGrabInteractable>());

        foreach (Transform child in transform)
        {
            if (child == null) { continue; }
            Destroy(child.gameObject);
        }

        Destroy(gameObject);
    }

    public void atomCollided(List<int> ids)
    {
        GameObject collidedAtom = atoms[ids[0]].GetComponent<Atom>().collided;

        addMolecule(ids[0], ids[1], collidedAtom.transform.parent.gameObject);
        atoms[ids[0]].GetComponent<Atom>().collided = null;
    }
}
