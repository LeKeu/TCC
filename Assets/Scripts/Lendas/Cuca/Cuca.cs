using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuca : MonoBehaviour
{
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
    bool acabouAtaque;

    #region Ataque InvocarMenino
    [SerializeField] GameObject MeninoInvocar;
    [SerializeField] Transform PosSpawnMenino;
    bool estaInvocandoMenino;
    #endregion

    private void Start()
    {
        fase = Fases.Fase1;
        ataque = Ataques.InvocarMenino;
    }

    private void Update()
    {
        if (fase == Fases.Fase1 && !chamandoFase1)
            StartCoroutine(Fase1());
    }

    IEnumerator Fase1()
    {
        chamandoFase1 = true;
        Debug.Log($"ataque atual --> {ataque}");
        switch (ataque)
        {
            case (Ataques.InvocarMenino):
                if (!estaInvocandoMenino) InvocarMenino();
                MudarAtaque();
                break;
            case (Ataques.Copias): Debug.Log("copias");
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

    void InvocarMenino()
    {
        estaInvocandoMenino = true;
        Instantiate(MeninoInvocar, PosSpawnMenino.position, Quaternion.identity);
        estaInvocandoMenino = false;
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