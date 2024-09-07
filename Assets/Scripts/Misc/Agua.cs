using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agua : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { collision.GetComponent<JogadorController>().estaNaAgua = true; }
        if(collision.gameObject.tag == "InimigoPadrao" || collision.gameObject.tag == "InimigoAtirador") { collision.GetComponent<InimigoPathFinding>().DiminuirVelocidade(); }
        if (collision.gameObject.tag == "InvocadoInimigo") { collision.GetComponent<InvocadoInimigo>().DiminuirVelocidade(); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { collision.GetComponent<JogadorController>().estaNaAgua = false; }
        if(collision.gameObject.tag == "InimigoPadrao" || collision.gameObject.tag == "InimigoAtirador") { collision.GetComponent<InimigoPathFinding>().VoltarVelocidadeNormal(); }
        if(collision.gameObject.tag == "InvocadoInimigo") { collision.GetComponent<InvocadoInimigo>().VoltarVelocidadeNormal(); }

    }
}
