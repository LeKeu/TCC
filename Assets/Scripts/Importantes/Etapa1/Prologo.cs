using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prologo : MonoBehaviour
{
    int totalNpcsConversaveis = 1;
    public static int qntdNpcsConversados = 0;

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
        if (qntdNpcsConversados == 1)
            Debug.Log("pode brigarrr");
    }

    public void CucaSequestraMenino()
    {
        Debug.Log("sequestro");
    }

    public void PerseguirCuca()
    {
        Debug.Log("perseguir");
    }
}
