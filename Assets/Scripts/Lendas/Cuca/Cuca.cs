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
    float velAux;

    BarraVidaBosses barraVidaBosses;
    Empurrao empurrao;
    Flash flash;

    int vidaAtual;
    string nome = "Cuca";
    bool derrotada;

    #region Movimentacao
    bool irAteJogador;
    bool estaPertoDoJogador;
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
    List<Ataques> ataquesLista = new List<Ataques>() { Ataques.InvocarMenino, Ataques.Copias, Ataques.DashSurpresa, Ataques.Impulso, Ataques.Distante }; 
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

        vidaAtual = VidaMaxFase1;
        velAux = velocidade;

        fase = Fases.Fase1;
        ataque = Ataques.Distante;
    }

    private void Update()
    {
        if (irAteJogador) // s� anda na dire��o do jogador se for verdadeiro
            IrAteJogadorFunc();
        ControleFases();
    }

    void IrAteJogadorFunc()
    {
        if(!estaPertoDoJogador)
            transform.position = Vector2.MoveTowards(this.transform.position, JogadorController.Instance.transform.position, velocidade * Time.deltaTime);

        if (Vector3.Distance(transform.position, JogadorController.Instance.transform.position) <= distMaxJogador)
            estaPertoDoJogador = true;
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
            case (Ataques.InvocarMenino): // s� na fase 2
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
        yield return new WaitForSeconds(2);
        chamandoFases = false;
    }

    void MudarAtaque() => ataque = ataquesLista[Random.Range(0, ataquesLista.Count)];
    void MudarAtaqueCopia() => ataque = ataquesListaCopia[Random.Range(0, ataquesListaCopia.Count)];

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
        irAteJogador = true; // dependendo de como for a mov da cuca, retirar isso
        velocidade = velocidadeDash;

        yield return new WaitForSeconds(tempoDash);

        velocidade = velAux;
        irAteJogador = false;
        estaDashing = false;
    }
    #endregion

    #region Ataque Impulsionar
    IEnumerator Impulsionar()
    {
        estaImpulsionando = true;
        irAteJogador = true;

        velocidade *= 2;

        yield return new WaitUntil(() => estaPertoDoJogador); // come�ar o ataque somente quando a cuca estiver perto do jogador
        yield return new WaitForSeconds(tempoEsperaImp);

        AtacarImpulsionar(danoImpulsionar);

        velocidade = velAux;

        irAteJogador = false;
        estaImpulsionando = false;
        estaPertoDoJogador = false;
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
        Debug.Log("atirando vezes "+ qndtProjetil);
        estaAtirandoProjetil = true;
        for (int i = 0; i < qndtProjetil; i++)
        {
            Instantiate(projetil, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(tempoEntreBalas);
        }
        estaAtirandoProjetil = false;
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
                }
                else Derrotar();
            }
        }
        else // CUCA C�PIA
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