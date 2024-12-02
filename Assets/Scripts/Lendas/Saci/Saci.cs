using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saci : MonoBehaviour
{
    BarraVidaBosses barraVidaBosses;

    [SerializeField] int Vida = 100;
    [SerializeField] GameObject bicho;
    //[SerializeField] float tempoDeVida = 5f;

    [SerializeField] List<GameObject> pontosSpawn = new List<GameObject>(); // dos invocados
    [SerializeField] List<GameObject> pontosSpawnSaci = new List<GameObject>();

    [SerializeField] List<GameObject> pontosSpawnEncontro1 = new List<GameObject>();


    public bool estaDerrotado;
    bool podeTeletransportar;
    bool estaAtordoado;

    int posAnterior = 0;

    string nome = "Saci";
    public static bool SACI_BR_FINALIZADO;

    private void Awake()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();
        
    }

    void Start()
    {
        //BatalhaBoss1();
    }

    #region TEMPORÁRIO PARA BOSSRUSH
    public IEnumerator SACI_BOSSRUSH()
    {
        BatalhaBoss1();
        yield return new WaitUntil(() => estaDerrotado);
        yield return new WaitForSeconds(2);
        SACI_BR_FINALIZADO = true;
    }
    #endregion

    public void IniciarBatalha_primeiroEncontroSaci()
    {
        SummonarBichosOrganizado(3, pontosSpawnEncontro1);
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
        if (collision.transform.tag == "FlechaPlayer")
        { // o dano da espada é feito no script "OrigemDano"
            if (estaAtordoado) { ReceberDano(2); }
        }
    }

    #region Boss -- Primeiro Encontro (invocar bichinhos e atordoar)
    public void BatalhaBoss1()
    {
        InvocadoInimigo.podeAndar = true;
        podeTeletransportar = true;
        estaAtordoado = false;
        StartCoroutine(ComecarBatalhaRoutine());
    }

    IEnumerator ComecarBatalhaRoutine()
    {
        if (!barraVidaBosses.ContainerEstaAtivo()) // criar a barra de vida do saci
            barraVidaBosses.CriarContainer(Vida, nome);

        if (Vida > 0)    // por x rounds, vai ter o ciclo de inst inimigos, derrotar, atordoar e repetir o round até que chegue a 0 a vida
        {
            StartCoroutine(SummonarBichos(3, pontosSpawn));
            
            barraVidaBosses.MudarCorBarra(Color.grey);
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length == 0);
            StartCoroutine(AtordoarSaciRoutine());
        }
        else Derrotado();

    }

    void Derrotado()
    {
        estaDerrotado = true;
        podeTeletransportar = false;
        barraVidaBosses.DesativarContainer();
    }

    IEnumerator AtordoarSaciRoutine()
    {
        podeTeletransportar = false;
        barraVidaBosses.MudarCorBarra(Color.green);

        yield return new WaitForSeconds(5);

        podeTeletransportar = true;
        StartCoroutine(ComecarBatalhaRoutine());
    }

    IEnumerator SummonarBichos(int qntd, List<GameObject> pos)
    {
        for (int i = 0; i < qntd; i++)
        {
            Instantiate(bicho, pos[Random.Range(0, 4)].transform);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    void SummonarBichosOrganizado(int qntd, List<GameObject> pos)
    {
        for (int i = 0; i < qntd; i++)
            Instantiate(bicho, pos[i].transform);
    }

    void Teletransportar()
    {
        int pos = Random.Range(0, 4);

        if (pos == posAnterior)
            pos += pos + 1 > 3 ? -1 : 1;

        gameObject.transform.position = pontosSpawnSaci[pos].transform.position;
        posAnterior = pos;
    }

    public void ReceberDano(int dano)
    {
        if(Vida >= 0)
        {
            Vida -= dano;
            barraVidaBosses.ReceberDano(dano);
        }
    }
}