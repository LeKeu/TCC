using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawnar : MonoBehaviour
{
    public void RespawnarJogador()
    {
        JogadorController.Instance.transform.position = gameObject.transform.position;
        ZerarEtapas();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ZerarEtapas()
    {
        Etapas.Prologo = false;
        Etapas.MeninaTocandoUkulele = false;
        Etapas.BrigaCelebracao = false;
        Etapas.CucaSequestro = false;
        Etapas.PrimeiroEncontroSaci = false;
        Etapas.BossSaci = false;
        Etapas.ETAPA3 = false;
        Etapas.ETAPA4 = false;
    }

    public void EsconderUI()
    {
        GameObject.Find("Canvas").SetActive(false);
    }
}
