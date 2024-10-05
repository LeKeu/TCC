using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Cuca : MonoBehaviour
{
    [SerializeField] bool copiaOriginal;
    [SerializeField] int VidaMaxFase1;
    [SerializeField] int VidaMaxFase2;
    BarraVidaBosses barraVidaBosses;
    int vidaAtual;
    string nome = "Cuca";
    bool derrotada;

    public enum Ataques
    {
        Copias,
        InvocarMenino,
        DashAleatorio,
        Impulso
    }
    List<Ataques> ataquesLista = new List<Ataques>() { Ataques.InvocarMenino, Ataques.Copias, Ataques.DashAleatorio, Ataques.Impulso }; 

    public enum Fases
    {
        Fase1,
        Fase2
    }

    Ataques ataque;
    Fases fase;
    bool chamandoFase1;
    bool chamandoFase2;

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

    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();
        vidaAtual = VidaMaxFase1;

        fase = Fases.Fase1;
        if (copiaOriginal)
            ataque = Ataques.Copias;
        else ataque = Ataques.InvocarMenino;
    }

    private void Update()
    {
        ControleFases();
    }

    void ControleFases()
    {
        if (!barraVidaBosses.ContainerEstaAtivo()) 
            barraVidaBosses.CriarContainer(VidaMaxFase1, nome);

        if (!derrotada)
        {
            if (fase == Fases.Fase1 && !chamandoFase1)
                StartCoroutine(FasesFunc(0));
        }
    }

    IEnumerator FasesFunc(int faseIndex)
    { // 1 fase1; 2 fase2
        chamandoFase1 = true;
        Debug.Log($"{copiaOriginal} atual --> {ataque}");
        switch (ataque)
        {
            case (Ataques.InvocarMenino): // só na fase 2
                if (!estaInvocandoMenino && fase == Fases.Fase2) StartCoroutine(InvocarMenino());
                MudarAtaque();
                break;
            case (Ataques.Copias):
                if (!estaCopiandoCuca && copiaOriginal) CopiarCuca();
                MudarAtaque();
                break;
            case (Ataques.DashAleatorio): Debug.Log("dash");
                MudarAtaque();
                break;
            case (Ataques.Impulso):
                Debug.Log("impulso");
                MudarAtaque();
                break;
        }
        //yield return new WaitUntil(() => acabouAtaque);
        yield return new WaitForSeconds(3);
        chamandoFase1 = false;
    }

    void MudarAtaque() => ataque = ataquesLista[Random.Range(0, ataquesLista.Count)];

    IEnumerator InvocarMenino()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer")
        {
            ReceberDano(2);
        }
    }

    public void ReceberDano(int dano)
    {
        if(vidaAtual > 0)
        {
            vidaAtual -= dano;
            barraVidaBosses.ReceberDano(dano);
            Debug.Log("vida atual "+vidaAtual);
        }
        else
        {
            if (fase == Fases.Fase1) 
            {
                Debug.Log("FASE 2 AGORA");
                fase = Fases.Fase2; 
                vidaAtual = VidaMaxFase2;
                Debug.Log($"vida atualç {vidaAtual}");
                barraVidaBosses.CriarContainer(VidaMaxFase2, nome);
            }
            else derrotada = true;
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