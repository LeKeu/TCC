using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{
    SeuJoao seuJoao;
    private void Start()
    {
        //seuJoao = GameObject.Find("SeuJoao").GetComponent<SeuJoao>();
        seuJoao = transform.GetComponentInParent<SeuJoao>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (JogadorController.Instance.estaEscondido)
        {
            seuJoao.tutCompleto = true;
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
