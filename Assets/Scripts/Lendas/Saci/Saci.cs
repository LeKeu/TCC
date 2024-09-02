using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saci : MonoBehaviour
{
    [SerializeField] int Vida = 20;
    [SerializeField] GameObject bicho;
    [SerializeField] float tempoDeVida = 5f;

    [SerializeField] List<GameObject> pontosSpawn = new List<GameObject>(); // dos invocados
    [SerializeField] List<GameObject> pontosSpawnSaci = new List<GameObject>();

    //List<GameObject> invocadosSummonados = new List<GameObject>();
    [SerializeField] int roundsInvocados = 4;
    bool podeSummonar;
    bool estaAtordoado;
    bool podeTeletransportar;

    int posAnterior = 0;

    void Start()
    {
        podeSummonar = true;
        estaAtordoado = false;
        podeTeletransportar = true;
    }

    private void FixedUpdate()
    {
        if (roundsInvocados > 0)
        {
            if (GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length == 0 && roundsInvocados != 4 && !podeSummonar)
            {
                estaAtordoado = true;
                StartCoroutine(AtordoarSaciRoutine());
            }
            if (!estaAtordoado && podeSummonar && GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length <= 3)
            {
                podeSummonar = false;
                StartCoroutine(SummonarBichoRoutine());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer" || collision.transform.tag == "Player")
        {
            if (podeTeletransportar) { Teletransportar(); }
            Debug.Log("podeSummonar = " + podeSummonar);
            Debug.Log("estaAtordoado = " + estaAtordoado);
            Debug.Log("roundsInvocados = " + roundsInvocados);
            Debug.Log("inimigos sumonados qntd = "+ GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer" || collision.transform.tag == "Player")
        {
            if (estaAtordoado) { ReceberDano(2); }
        }
    }

    void Teletransportar()
    {
        int pos = Random.Range(0, 4);

        if (pos == posAnterior)
            pos += pos + 1 > 3 ? -1 : 1;

        gameObject.transform.position = pontosSpawnSaci[pos].transform.position;
        posAnterior = pos;
    }

    #region Summonar Invocado (Bichinho)
    IEnumerator SummonarBichoRoutine()
    {
        podeSummonar = false;
        SummonarBicho();
        yield return new WaitForSeconds(1);
        SummonarBicho();
        yield return new WaitForSeconds(1);
        SummonarBicho();
    }
    void SummonarBicho()
    {
        Debug.Log("SUMONANDO");
        //podeSummonar = false;
        if(GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length < 3)
            Instantiate(bicho, pontosSpawn[Random.Range(0, 4)].transform);
            //invocadosSummonados.Add(bichoSummonado);
    }
    #endregion

    #region Atordoar Saci
    IEnumerator AtordoarSaciRoutine()
    {
        podeSummonar = false;
        estaAtordoado = true;
        podeTeletransportar = false;

        yield return new WaitForSeconds(5);

        podeSummonar = true;
        estaAtordoado = false;
        podeTeletransportar = true;
    }

    void ReceberDano(int dano)
    {
        Vida -= dano;
        //Debug.Log("vida = " + Vida);
    }
    #endregion
}