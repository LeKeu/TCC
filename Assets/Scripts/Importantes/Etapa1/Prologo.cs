using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prologo : MonoBehaviour
{
    int totalNpcsConversaveis = 7;
    public static int qntdNpcsConversados = 0;
    bool aconteceuBriga;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerBriga")
            Brigar();
        if(collision.GetComponent<JogadorController>() && gameObject.name == "TriggerCucaSeq")
            CucaSequestraMenino();
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerPerseguirCuca")
            PerseguirCuca();
    } 

    public void Brigar()
    {
        if (qntdNpcsConversados == totalNpcsConversaveis)
            aconteceuBriga = true;
    }

    public void CucaSequestraMenino()
    {
        if(aconteceuBriga)
            Debug.Log("sequestro");
    }

    public void PerseguirCuca()
    {
        Debug.Log("perseguir");
    }
}