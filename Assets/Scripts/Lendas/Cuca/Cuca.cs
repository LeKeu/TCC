using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuca : MonoBehaviour
{
    [Header("Geral")]
    [SerializeField] bool copiaOriginal;
    [SerializeField] int VidaMaxFase1;
    [SerializeField] int VidaMaxFase2;
    [SerializeField] int vidaCopias;
    [SerializeField] float velocidade;
    float tempoEntreAtaques = 5f;
    float velAux;

    BarraVidaBosses barraVidaBosses;
    Empurrao empurrao;
    Flash flash;

    int vidaAtual;
    string nome = "Cuca";
    bool derrotada;

    #region Movimentacao
    [SerializeField] float distMaxJogadorNormal = 5f;
    [SerializeField] float tempoParadaEmAtaque = 2f;
    bool irAteJogador;
    bool estaPertoDoJogadorAtaque;
    bool estaPertoDoJogadorNormal;
    bool ataqueDeMovimento;
    bool podeMover;
    #endregion

    #region Enums
    public enum Ataques
    {
        Copias,
        InvocarMenino,
        DashSurpresa,
        Impulso,
        Distante
    }
    List<Ataques> ataquesLista = new List<Ataques>() { Ataques.DashSurpresa, Ataques.Impulso, Ataques.Distante, Ataques.Copias, Ataques.InvocarMenino }; 
    List<Ataques> ataquesListaCopia = new List<Ataques>() { Ataques.DashSurpresa, Ataques.Impulso, Ataques.Distante }; 

    public enum Fases
    {
        Fase1,
        Fase2
    }
    #endregion

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
    [SerializeField] List<Transform> pontosSpawnCopias;
    bool estaCopiandoCuca;
    #endregion

    #region DashSurpresa
    [Header("DashSurpresa")]
    [SerializeField] float velocidadeDash;
    [SerializeField] float tempoDash;
    bool estaDashing;
    #endregion

    #region Impulsionar
    [Header("Impulsionar")]
    [SerializeField] int danoImpulsionar = 4;
    [SerializeField] float tamanhoAreaAtaque = 3f;
    [SerializeField] float tempoEsperaImp = 1.5f;
    [SerializeField] float distMaxJogador = 1f;
    bool estaImpulsionando;
    #endregion

    #region Distancia
    [Header("Distante")]
    [SerializeField] GameObject projetil;
    [SerializeField] int qndtProjetil = 6;
    [SerializeField] float tempoEntreBalas = .5f;
    bool estaAtirandoProjetil;
    #endregion

    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();
        empurrao = GetComponent<Empurrao>();
        flash = GetComponentInChildren<Flash>();
        podeMover = true;

        vidaAtual = VidaMaxFase1;
        velAux = velocidade;

        fase = Fases.Fase1;
        ataque = Ataques.Distante;
    }

    private void Update()
    {
        if (podeMover)
            Movimentar();

        ControleFases();
    }

    #region Movimentação
    void Movimentar()
    {
        if(!ataqueDeMovimento)
            IrLongeJogador();
        else
            IrAteJogadorFunc();
    }

    void IrAteJogadorFunc()
    {
        if(!estaPertoDoJogadorAtaque)
            transform.position = Vector2.MoveTowards(this.transform.position, JogadorController.Instance.transform.position, velocidade * Time.deltaTime);

        if (Vector3.Distance(transform.position, JogadorController.Instance.transform.position) <= distMaxJogador)
            estaPertoDoJogadorAtaque = true;
    }

    void IrLongeJogador()
    {
        if (Vector3.Distance(transform.position, JogadorController.Instance.transform.position) < distMaxJogadorNormal)
        {
            estaPertoDoJogadorNormal = true;
        }

        if (estaPertoDoJogadorNormal)
        {
            Vector2 direcaoOposta = (transform.position - JogadorController.Instance.transform.position).normalized;

            transform.position = Vector2.MoveTowards(this.transform.position, this.transform.position + (Vector3)direcaoOposta, velocidade * Time.deltaTime);
        }

        // Verifica se o objeto está longe o suficiente do jogador para parar de se mover
        if (Vector3.Distance(transform.position, JogadorController.Instance.transform.position) >= distMaxJogadorNormal)
        {
            estaPertoDoJogadorNormal = false;
        }
    }
    #endregion

    void MudarStatsFase2()
    {
        velocidade += 2;
        velAux = velocidade;
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

        if(copiaOriginal) Debug.Log(ataque);

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

            case (Ataques.Distante):
                if (!estaAtirandoProjetil)
                    StartCoroutine(CriarProjetilDistante());
                break;
        }

        if (copiaOriginal) MudarAtaque();
        else MudarAtaqueCopia();

        //yield return new WaitUntil(() => acabouAtaque);
        yield return new WaitForSeconds(tempoEntreAtaques);
        chamandoFases = false;
    }

    void MudarAtaque() => ataque = fase == Fases.Fase1?  ataquesLista[Random.Range(0, ataquesLista.Count-1)] : ataquesLista[Random.Range(0, ataquesLista.Count)];
    void MudarAtaqueCopia() => ataque = ataquesListaCopia[Random.Range(0, ataquesListaCopia.Count)];

    #region Ataque Invocar Menino
    IEnumerator InvocarMenino()
    {
        StartCoroutine(PararPAtacar());

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
        StartCoroutine(PararPAtacar());

        if (GameObject.FindGameObjectsWithTag("CucaCopia").Length == 0)
        {
            estaCopiandoCuca = true;
            Instantiate(CucaCopia, pontosSpawnCopias[0].position, Quaternion.identity);
            Instantiate(CucaCopia, pontosSpawnCopias[1].position, Quaternion.identity);
            Instantiate(CucaCopia, pontosSpawnCopias[2].position, Quaternion.identity);
            estaCopiandoCuca = false;
        }
    }
    #endregion

    #region Ataque Dash Surpresa
    IEnumerator DashSurpresa()
    {
        ataqueDeMovimento = true;
        estaDashing = true;
        irAteJogador = true; // dependendo de como for a mov da cuca, retirar isso
        velocidade = velocidadeDash;

        yield return new WaitForSeconds(tempoDash);

        velocidade = velAux;
        irAteJogador = false;
        estaDashing = false;
        ataqueDeMovimento = false;
    }
    #endregion

    #region Ataque Impulsionar
    IEnumerator Impulsionar()
    {
        ataqueDeMovimento = true;
        estaImpulsionando = true;
        irAteJogador = true;

        velocidade *= 2;

        yield return new WaitUntil(() => estaPertoDoJogadorAtaque); // começar o ataque somente quando a cuca estiver perto do jogador
        yield return new WaitForSeconds(tempoEsperaImp);

        AtacarImpulsionar(danoImpulsionar);

        velocidade = velAux;

        irAteJogador = false;
        estaImpulsionando = false;
        ataqueDeMovimento = false;
        estaPertoDoJogadorAtaque = false;
    }

    void AtacarImpulsionar(int dano)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, tamanhoAreaAtaque);

        foreach (Collider2D c in colliders)
        {
            if (c.GetComponent<JogadorVida>())
            {
                c.GetComponent<JogadorVida>().LevarDano(dano);
                c.GetComponent<JogadorVida>().EmpurrarPlayer(gameObject.transform, 20f);
                break;
            }
        }
    }
    #endregion

    #region Ataque Distante
    IEnumerator CriarProjetilDistante()
    {
        //Debug.Log("atirando vezes "+ qndtProjetil);
        estaAtirandoProjetil = true;
        for (int i = 0; i < qndtProjetil; i++)
        {
            Instantiate(projetil, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(tempoEntreBalas);
        }
        estaAtirandoProjetil = false;
    }
    #endregion

    IEnumerator PararPAtacar()
    {
        podeMover = false;
        yield return new WaitForSeconds(tempoParadaEmAtaque);
        podeMover = true;
    }

    public void ReceberDano(int dano)
    {
        if (copiaOriginal) // CUCA
        {
            if (vidaAtual > 0)
            {
                vidaAtual -= dano;
                barraVidaBosses.ReceberDano(dano);

                StartCoroutine(flash.FlashRoutine());
            }
            else // se a vida for menor que 0
            {
                if (fase == Fases.Fase1)
                {
                    IniciarFase2();
                    MudarStatsFase2();
                }
                else Derrotar();
            }
        }
        else // CUCA CÓPIA
        {
            if (vidaCopias > 0)
            {
                vidaCopias -= dano;
                StartCoroutine(flash.FlashRoutine());
            }
            else Destroy(this.gameObject);
        }
    }

    void Derrotar()
    {
        derrotada = true;
        GameObject[] copias = GameObject.FindGameObjectsWithTag("CucaCopia");
        if(copias.Length != 0)
        {
            foreach (GameObject copia in copias)
                Destroy(copia);
        }
        StopAllCoroutines();
    }

    void IniciarFase2()
    {
        fase = Fases.Fase2;
        vidaAtual = VidaMaxFase2;
        barraVidaBosses.CriarContainer(VidaMaxFase2, nome);
        barraVidaBosses.MudarCorBarra(Color.red);
        //barraVidaBosses.ReceberDano(dano);
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