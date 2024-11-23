using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUX_NPCS_RESOURCES : MonoBehaviour
{
    public static Sprite[] perfilsNPCs = {};
    void Start()
    {
        perfilsNPCs = Resources.LoadAll<Sprite>("NPCS_PERFILS");
    }
}
