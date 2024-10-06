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

    bool irAteJogador;

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

    #region Impulsionar
    [Header("Impulsionar")]
    [SerializeField] int danoImpulsionar;
    [SerializeField] float tamanhoAreaAtaque = 3f;
    [SerializeField] float tempoEsperaImp;
    bool estaImpulsionando;
    #endregion

    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();
        vidaAtual = VidaMaxFase1;
        velAux = velocidade;

        fase = Fases.Fase1;
        if (copiaOriginal)
            ataque = Ataques.Impulso;
        else ataque = Ataques.InvocarMenino;
    }

    private void Update()
    {
        if(irAteJogador) // só anda na direção do jogador se for verdadeiro
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
                if (!estaInvocandoMenino && fase == Fases.Fase2 && copiaOriginal) 
                    StartCoroutine(InvocarMenino());
                break;
            case (Ataques.Copias):
                if (!estaCopiandoCuca && copiaOriginal) 
                    CopiarCuca();
                break;
            case (Ataques.DashSurpresa): 
                if(!estaDashing) 
                    StartCoroutine(DashSurpresa());
                break;
            case (Ataques.Impulso):
                if (!estaImpulsionando) 
                    StartCoroutine(Impulsionar());
                break;
        }

        //MudarAtaque();
        //yield return new WaitUntil(() => acabouAtaque);
        yield return new WaitForSeconds(1);
        chamandoFases = false;
    }

    void MudarAtaque() => ataque = ataquesLista[Random.Range(0, ataquesLista.Count)];
    void IrAteJogador(bool valor) => irAteJogador = valor;

    #region Ataque Invocar Menino
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
    #endregion

    #region Ataque Copiar Cuca
    void CopiarCuca()
    {
        if(GameObject.FindGameObjectsWithTag("CucaCopia").Length < 3)
        {
            estaCopiandoCuca = true;
            Instantiate(CucaCopia, PosSpawnMenino.position, Quaternion.identity);
            estaCopiandoCuca = false;
        }
    }
    #endregion

    #region Ataque Dash Surpresa
    IEnumerator DashSurpresa()
    {
        estaDashing = true;
        velocidade = velocidadeDash;
        yield return new WaitForSeconds(tempoDash);
        velocidade = velAux;
        estaDashing = false;
    }
    #endregion

    #region Ataque Impulsionar
    IEnumerator Impulsionar()
    {
        estaImpulsionando = true;
        irAteJogador = true;

        yield return new WaitForSeconds(tempoEsperaImp);
        
        AtacarImpulsionar(danoImpulsionar);
        irAteJogador = false;
        estaImpulsionando = false;
    }

    void AtacarImpulsionar(int dano)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, 3f);

        foreach (Collider2D c in colliders)
        {
            if (c.GetComponent<JogadorVida>())
            {
                c.GetComponent<JogadorVida>().LevarDano(dano);
                c.GetComponent<JogadorVida>().EmpurrarPlayer(gameObject.transform, 20f);
            }
        }
    }
    #endregion

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
                    barraVidaBosses.ReceberDano(dano);
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