using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeIDManager : MonoBehaviour
{
    private int nextMoleculeID = 0;

    public int getNextID() { return nextMoleculeID++; }
}
