using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarExtras : MonoBehaviour
{
    [SerializeField] private GameObject moedaPrefab;

    public void DroparItens()
    {
        Instantiate(moedaPrefab, transform.position, Quaternion.identity);
    }
}
