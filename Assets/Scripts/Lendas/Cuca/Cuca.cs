using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Cuca : MonoBehaviour
{
    [Header("Geral")]
    [SerializeField] bool copiaOriginal;
    [SerializeField] int VidaMaxFase1;
    [SerializeField] int VidaMaxFase2;
    [SerializeField] int vidaCopias;
    [SerializeField] float velocidade;

    BarraVidaBosses barraVidaBosses;
    int vidaAtual;
    string nome = "Cuca";
    bool derrotada;

    public enum Ataques
    {
        Copias,
        InvocarMenino,
        DashSurpresa,
        Impulso
    }
    List<Ataques> ataquesLista = new List<Ataques>() { Ataques.InvocarMenino, Ataques.Copias, Ataques.DashSurpresa, Ataques.Impulso }; 

    public enum Fases
    {
        Fase1,
        Fase2
    }

    Ataques ataque;
    Fases fase;
    bool chamandoFases;

    #region Ataque InvocarMenino
    [Header("AtaqueInvocar")]
    [SerializeField] GameObject MeninoInvocar;
    [SerializeField] Transform PosSpawnMenino;
    [SerializeField] int qntdMeninos = 2; // fazer isso
    bool estaInvocandoMenino;
    #endregion

    #region Ataque Copia
    [Header("AtaqueCopiar")]
    [SerializeField] GameObject CucaCopia;
    bool estaCopiandoCuca;
    #endregion

    #region DashSurpresa
    [Header("DashSurpresa")]
    [SerializeField] float velocidadeDash;
    [SerializeField] float tempoDash;
    float velAux;
    bool estaDashing;
    #endregion

    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();
        vidaAtual = VidaMaxFase1;
        velAux = velocidade;

        fase = Fases.Fase1;
        if (copiaOriginal)
            ataque = Ataques.Copias;
        else ataque = Ataques.InvocarMenino;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(this.transform.position, JogadorController.Instance.transform.position, velocidade * Time.deltaTime);
        ControleFases();
    }

    void ControleFases()
    {
        if (!barraVidaBosses.ContainerEstaAtivo()) 
            barraVidaBosses.CriarContainer(VidaMaxFase1, nome);

        if (!derrotada)
        {
            if (fase == Fases.Fase1 && !chamandoFases)
                StartCoroutine(FasesFunc(0));
            if (fase == Fases.Fase2 && !chamandoFases)
                StartCoroutine(FasesFunc(1));
        }
    }

    IEnumerator FasesFunc(int faseIndex)
    { // 0 fase1; 1 fase2
        chamandoFases = true;
        //Debug.Log(ataque);
        switch (ataque)
        {
            case (Ataques.InvocarMenino): // só na fase 2
                if (!estaInvocandoMenino && fase == Fases.Fase2 && copiaOriginal) StartCoroutine(InvocarMenino());
                MudarAtaque();
                break;
            case (Ataques.Copias):
                if (!estaCopiandoCuca && copiaOriginal) CopiarCuca();
                MudarAtaque();
                break;
            case (Ataques.DashSurpresa): 
                if(!estaDashing) StartCoroutine(DashSurpresa());
                MudarAtaque();
                break;
            case (Ataques.Impulso):
                MudarAtaque();
                break;
        }
        //yield return new WaitUntil(() => acabouAtaque);
        yield return new WaitForSeconds(1);
        chamandoFases = false;
    }

    void MudarAtaque() => ataque = ataquesLista[Random.Range(0, ataquesLista.Count)];

    IEnumerator InvocarMenino()
    {
        if (!copiaOriginal)
            Debug.Log("copia incvocando menino");
        estaInvocandoMenino = true;
        for(int i = 0; i < qntdMeninos; i++)
        {
            Instantiate(MeninoInvocar, PosSpawnMenino.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        estaInvocandoMenino = false;
    }

    void CopiarCuca()
    {
        if(GameObject.FindGameObjectsWithTag("CucaCopia").Length < 3)
        {
            estaCopiandoCuca = true;
            Instantiate(CucaCopia, PosSpawnMenino.position, Quaternion.identity);
            estaCopiandoCuca = false;
        }
    }

    IEnumerator DashSurpresa()
    {
        estaDashing = true;
        velocidade = velocidadeDash;
        yield return new WaitForSeconds(tempoDash);
        velocidade = velAux;
        estaDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer")
        {
            ReceberDano(2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer")
        {
            ReceberDano(2);
        }
    }

    public void ReceberDano(int dano)
    {
        if (copiaOriginal)
        {
            if (vidaAtual > 0)
            {
                vidaAtual -= dano;
                barraVidaBosses.ReceberDano(dano);
                //Debug.Log("vida atual " + vidaAtual);
            }
            else
            {
                if (fase == Fases.Fase1)
                {
                    fase = Fases.Fase2;
                    vidaAtual = VidaMaxFase2;
                    barraVidaBosses.CriarContainer(VidaMaxFase2, nome);
                }
                else derrotada = true;
            }
        }
        else
        {
            if (vidaCopias > 0)
                vidaCopias -= dano;
            else Destroy(this.gameObject);
        }
    }
}


/* PARA SEMPRE MENINOS EM TODO LUGAR FOREVER TOUJOUR
     private void Update()
    {
        if (fase == Fases.Fase1)
            StartCoroutine(Fase1());
    }

        void Fase1()
    {
        Debug.Log($"ataque atual --> {ataque}");
        switch (ataque)
        {
            case (Ataques.InvocarMenino):
                if (!estaInvocandoMenino) InvocarMenino();
                MudarAtaque();
                break;
            case (Ataques.Copias):
                Debug.Log("copias");
                MudarAtaque();
                break;
            case (Ataques.DashAleatorio):
                Debug.Log("dash");
                MudarAtaque();
                break;
            case (Ataques.Impulso):
                Debug.Log("impulso");
                MudarAtaque();
                break;
        }
    }
     */