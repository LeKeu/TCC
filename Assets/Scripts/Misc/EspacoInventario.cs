using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspacoInventario : MonoBehaviour
{

    [SerializeField] private Armas armas;

    public Armas PegarArmaInfo()
    {
        return armas;
    }
}
