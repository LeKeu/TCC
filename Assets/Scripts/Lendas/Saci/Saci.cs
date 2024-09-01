using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saci : MonoBehaviour
{
    [SerializeField] GameObject bicho;
    [SerializeField] float tempoDeVida = 5f;

    bool primeiraInteracao;
    bool podeTeletransportar;

    [SerializeField] List<GameObject> pontosSpawn = new List<GameObject>(); // dos invocados
    [SerializeField] List<GameObject> pontosSpawnSaci = new List<GameObject>();

    List<GameObject> invocadosSummonados = new List<GameObject>();

    int posAnterior = 0;
    // Start is called before the first frame update
    void Start()
    {
        podeTeletransportar = true;
        //StartCoroutine(SummonarBichoRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "FlechaPlayer" || collision.transform.tag == "Player")
        {
            if(podeTeletransportar) { Teletransportar(); }
        }
    }

    void Teletransportar()
    {
        int pos = Random.Range(0, 4);

        if(pos == posAnterior)
            pos += 1 > 3 ? pos -= 1 : pos += 1;

        Debug.Log(pos);
        gameObject.transform.position = pontosSpawnSaci[pos].transform.position;
        posAnterior = pos;
    }

    #region Summonar Invocado (Bichinho)
    IEnumerator SummonarBichoRoutine()
    {
        SummonarBicho(3);
        yield return new WaitForSeconds(tempoDeVida);
        foreach (GameObject bicho in invocadosSummonados)
            Destroy(bicho);
    }

    void SummonarBicho(int qntd)
    {
        for(int i = 0; i < qntd; i++)
        {
            GameObject bichoSummonado = Instantiate(bicho, pontosSpawn[i].transform);
            invocadosSummonados.Add(bichoSummonado);
        }
    }
    #endregion
}
