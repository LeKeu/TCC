using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pontes : MonoBehaviour
{
    GameObject AguaFundoGrid;

    private void Start()
    {
        AguaFundoGrid = GameObject.Find("AguaFundo");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorController>())
        {
            AguaFundoGrid.GetComponent<TilemapCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorController>())
        {
            AguaFundoGrid.GetComponent<TilemapCollider2D>().enabled = true;

        }
    }
}
