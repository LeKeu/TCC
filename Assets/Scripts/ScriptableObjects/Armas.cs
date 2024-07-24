using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nova Arma")]
public class Armas : ScriptableObject
{
    public GameObject armaPrefab;
    public float armaCooldown;
    public float armaRange;
    public int armaDano;
}
