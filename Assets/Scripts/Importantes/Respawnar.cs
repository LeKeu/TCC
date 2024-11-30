using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawnar : MonoBehaviour
{
    LuzesCiclo luzesCiclo;
    bool chamouMorrer;

    private void Start()
    {
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();
    }

    private void Update()
    {
        if (!JogadorVida.estaViva && !chamouMorrer)
            Morrer();
    }

    void Morrer()
    {
        chamouMorrer = true;
        EsconderUI();

        JogadorController.Instance.podeMover = false;
        luzesCiclo.MudarCorAmbiente(Color.black, 5f);
        RespawnarJogador();
    }

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
