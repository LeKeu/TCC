using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{
    SeuJoao seuJoao;
    Pedrinho pedrinho;
    private void Start()
    {
        //seuJoao = GameObject.Find("SeuJoao").GetComponent<SeuJoao>();
        seuJoao = transform.GetComponentInParent<SeuJoao>();
        pedrinho = GameObject.Find("Pedrinho").GetComponent<Pedrinho>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (JogadorController.Instance.estaEscondido && pedrinho.podePegarBola) // se eu já tiver falado com o pedrinho p pegar a bola
        {
            seuJoao.CompletarTutorial();
            pedrinho.CompletarTutorial();
            Destroy(this.gameObject);
        }
        //if (seuJoao.podePegarBola && !seuJoao.foiPego)
        //{
        //    seuJoao.tutCompleto = true;
        //    Debug.Log($"pode pegar bola = "+seuJoao.podePegarBola);
        //    Debug.Log($"foi pego = "+seuJoao.foiPego);
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    Debug.Log("foi pego, bola nao");
        //}
    }
}
