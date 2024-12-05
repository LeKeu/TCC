using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawnar : MonoBehaviour
{
    [SerializeField] InventarioAtivo inventarioAtivo;

    LuzesCiclo luzesCiclo;
    GameObject canvas;
    bool chamouMorrer;
    int vidaTemp;

    private void Start()
    {
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();
        canvas = GameObject.Find("Canvas");
        vidaTemp = JogadorVida.vidaAtual;
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
        //ArmaAtiva.Instance.AtivarArma1(false);

        JogadorController.Instance.podeMover = false;
        luzesCiclo.MudarCorAmbiente(Color.black, 5f);
        StartCoroutine(RespawnarJogador());
    }

    IEnumerator RespawnarJogador()
    {
        ZerarEtapas();
        yield return new WaitForSeconds(5);
        JogadorController.Instance.transform.position = gameObject.transform.position;
        JogadorVida.vidaAtual = vidaTemp;
        if (SceneManager.GetActiveScene().name == "BOSSRUSH")
        {
            //inventarioAtivo.DesativarArma();

            Saci.SACI_BR_FINALIZADO = false;
            Iara.IARA_BR_FINALIZADO = false;
            Cuca.CUCA_BR_FINALIZADO = false;

            SceneManager.LoadScene("Inicio");
            yield break;
        }
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
        canvas.SetActive(false);
    }
}
