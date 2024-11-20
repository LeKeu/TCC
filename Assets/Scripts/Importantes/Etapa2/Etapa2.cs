using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Etapa2 : MonoBehaviour
{
    [SerializeField] GameObject Saci;

    [Header("Geral")]
    [SerializeField] SFX sfx_script;
    [SerializeField] Tutorial tutorial_script;
    [SerializeField] ArmaAtiva armaAtiva;
    [SerializeField] InventarioAtivo inventarioAtivo;


    #region Primeiro Encontro Saci
    [Header("Primeiro Encontro Saci")]
    [SerializeField] AudioClip SaciAssobio;
    [SerializeField] Transform posMenina_ConversaSaci;
    [SerializeField] Transform posMenina_AcabouLuta;
    [SerializeField] GameObject barreiraMoitasVermelhas;

    bool aconteceuEncontro;
    bool meninaAndando_EncontroSaci;
    bool meninaAndando_AcabouLuta;
    #endregion
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_saci")
        {
            JogadorController.Instance.velocidade = 1;
            sfx_script.FlorestaNoite();
            Saci.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Debug.Log(JogadorController.Instance.velocidade);  
        if(Etapas.PrimeiroEncontroSaci && this.gameObject.name == "PrimeiroEncontroSaci")
        {
            if(meninaAndando_EncontroSaci)
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMenina_ConversaSaci.position, 1 * Time.deltaTime);
            if(meninaAndando_AcabouLuta)
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMenina_AcabouLuta.position, 1 * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<JogadorController>() && gameObject.name == "PrimeiroEncontroSaci" && !aconteceuEncontro)
        {
            StartCoroutine(PrimeiroEncontroSaci());
        }
    }

    IEnumerator PrimeiroEncontroSaci()
    {
        Etapas.PrimeiroEncontroSaci = true;
        aconteceuEncontro = true;

        #region  menina andando e escuta assobio
        meninaAndando_EncontroSaci = true;

        sfx_script.AssobioSaci();
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_ConversaSaci.transform.position) < 1);
        //yield return new WaitForSeconds(10);
        meninaAndando_EncontroSaci = false;
        MudarEstadoJogador(false);
        #endregion

        #region encontrando o saci
        sfx_script.PararAssobioSaci();
        Saci.SetActive(true);
        yield return new WaitForSeconds(3);

        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        Saci.GetComponent<Saci>().IniciarBatalha_primeiroEncontroSaci();
        InvocadoInimigo.podeAndar = false;
        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        InvocadoInimigo.podeAndar = true;
        #endregion

        #region ativar arma, tutorial purificacao, inicio batalha saci
        armaAtiva.AtivarArma1(true);
        inventarioAtivo.AtivarArma1(true);

        tutorial_script.IniciarTutorial_PararTempo("Aperte 'Q' para purificar o inimigo.", KeyCode.Q);
        yield return new WaitUntil(() => !tutorial_script.duranteTutorial);

        MudarEstadoJogador(true);
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length == 0);
        #endregion

        MudarEstadoJogador(false); // checar aqui!! o player ta andando estranho ás vezes

        #region acabou luta, dialogo saci
        yield return new WaitForSeconds(3);

        meninaAndando_AcabouLuta = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_AcabouLuta.transform.position) < 1);
        
        yield return new WaitForSeconds(3);

        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 2);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        Saci.SetActive(false);
        barreiraMoitasVermelhas.SetActive(false);
        #endregion 

        MudarEstadoJogador(true);
        Etapas.PrimeiroEncontroSaci = false;
        JogadorController.Instance.velocidade = 4f;
    }

    void Interagir_Geral(MonoBehaviour script, int index, string metodoNome = "Interagir")
    {
        object[] parametros = { index };
        //Debug.Log(script.name);
        var metodo = script.GetType().GetMethod($"{metodoNome}");
        metodo.Invoke(script, parametros);
    }
    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
        //JogadorController.Instance.podeFlipX = !acao;
    }
}
