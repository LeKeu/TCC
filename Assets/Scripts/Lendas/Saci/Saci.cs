using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saci : MonoBehaviour
{
    #region Situações da História
    bool primeiroEncontro;
    bool comecouPrimeiroEncontro;

    #endregion

    [SerializeField] int Vida = 20;
    [SerializeField] GameObject bicho;
    [SerializeField] float tempoDeVida = 5f;

    [SerializeField] List<GameObject> pontosSpawn = new List<GameObject>(); // dos invocados
    [SerializeField] List<GameObject> pontosSpawnSaci = new List<GameObject>();

    //List<GameObject> invocadosSummonados = new List<GameObject>();
    [SerializeField] int roundsInvocados = 4;
    bool podeTeletransportar;
    bool estaAtordoado;

    int posAnterior = 0;

    void Start()
    {
        podeTeletransportar = true;
        estaAtordoado = false;
        //StartCoroutine(ComecarBatalhaRoutine());
    }

    private void Update()
    {
        if(primeiroEncontro && !comecouPrimeiroEncontro) // situação da primeira vez encontrando o saci
        { comecouPrimeiroEncontro = true; StartCoroutine(ComecarBatalhaRoutine()); }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer" || collision.transform.tag == "Player")
        {
            if (podeTeletransportar) { Teletransportar(); }
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

    IEnumerator ComecarBatalhaRoutine()
    {
        if(Vida > 0)    // por x rounds, vai ter o ciclo de inst inimigos, derrotar, atordoar e repetir o round até que chegue a 0
        {
            SummonarBichos(1);
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length == 0);
            Debug.Log("todos m,ortos");
            StartCoroutine(AtordoarSaciRoutine());
        }
    }

    IEnumerator AtordoarSaciRoutine()
    {
        roundsInvocados--;
        podeTeletransportar = false;

        yield return new WaitForSeconds(5);

        podeTeletransportar = true;
        StartCoroutine(ComecarBatalhaRoutine());
    }

    void SummonarBichos(int qntd)
    {
        for (int i = 0; i < qntd; i++)
            Instantiate(bicho, pontosSpawn[Random.Range(0, 4)].transform);
    }

    public void ReceberDano(int dano)
    {
        Vida -= dano;
        Debug.Log("vida = " + Vida);
    }
}