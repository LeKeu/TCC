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
        if(collision.gameObject.tag == "Player") { JogadorController.Instance.DiminuirVelocidade(); Debug.Log("nadando"); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { JogadorController.Instance.VoltarVelocidadeNormal(); }
    }
}
