using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agua : MonoBehaviour
{
    TilemapCollider2D tc;
    // Start is called before the first frame update
    void Start()
    {
        tc = GetComponent<TilemapCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player") { Debug.Log("jogador"); }
    }
}
