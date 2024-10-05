using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuca : MonoBehaviour
{
    [SerializeField] bool copiaOriginal;
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

    #region Ataque InvocarMenino
    [SerializeField] GameObject MeninoInvocar;
    [SerializeField] Transform PosSpawnMenino;
    [SerializeField] int qntdMeninos = 2; // fazer isso
    bool estaInvocandoMenino;
    #endregion

    #region Ataque Copia
    [SerializeField] GameObject CucaCopia;
    bool estaCopiandoCuca;
    #endregion

    private void Start()
    {
        fase = Fases.Fase1;
        if (copiaOriginal)
            ataque = Ataques.Copias;
        else ataque = Ataques.InvocarMenino;
    }

    private void Update()
    {
        if (fase == Fases.Fase1 && !chamandoFase1)
            StartCoroutine(Fase1());
    }

    IEnumerator Fase1()
    {
        chamandoFase1 = true;
        Debug.Log($"{copiaOriginal} atual --> {ataque}");
        switch (ataque)
        {
            case (Ataques.InvocarMenino):
                if (!estaInvocandoMenino) StartCoroutine(InvocarMenino());
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
        estaCopiandoCuca = true;
        Instantiate(CucaCopia, PosSpawnMenino.position, Quaternion.identity);
        estaCopiandoCuca = false;
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