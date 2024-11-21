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

    #region Boss Saci
    [Header("Boss Saci")]
    [SerializeField] Transform posMenina_BossSaci;
    [SerializeField] Transform posMenina_DepoisBossSaci;
    [SerializeField] Transform posSaci_DepoisBossSaci;

    bool aconteceuBossSaci;
    bool meninaAndando_inicioBossSaci;
    bool meninaAndando_depoisBossSaci;
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
        if(Etapas.PrimeiroEncontroSaci && this.gameObject.name == "PrimeiroEncontroSaci")
        {
            if (meninaAndando_EncontroSaci)
                MovimentarGameObject(JogadorController.Instance.gameObject, posMenina_ConversaSaci, 1);
            if (meninaAndando_AcabouLuta)
                MovimentarGameObject(JogadorController.Instance.gameObject, posMenina_AcabouLuta, 1);

        }

        if (Etapas.BossSaci && this.gameObject.name == "BossSaci")
        {
            if (meninaAndando_inicioBossSaci)
                MovimentarGameObject(JogadorController.Instance.gameObject, posMenina_BossSaci, 1);
            if (meninaAndando_depoisBossSaci)
                MovimentarGameObject(JogadorController.Instance.gameObject, posMenina_DepoisBossSaci, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("colidiu");
        if(collision.GetComponent<JogadorController>() && gameObject.name == "PrimeiroEncontroSaci" && !aconteceuEncontro)
        {
            StartCoroutine(PrimeiroEncontroSaci());
        }

        if (collision.GetComponent<JogadorController>() && gameObject.name == "BossSaci" && !aconteceuBossSaci)
        {
            StartCoroutine(BossSaci());
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

    IEnumerator BossSaci()
    {
        aconteceuBossSaci = true;
        Etapas.BossSaci = true;

        #region ir em direção do saci, dialogo começa
        meninaAndando_inicioBossSaci = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_BossSaci.transform.position) < 1);
        meninaAndando_inicioBossSaci = false;

        MudarEstadoJogador(false);
        yield return new WaitForSeconds(2); // middle button n ta passando dialogo p esse saciii

        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region começar boss, esperar derrota saci
        yield return new WaitForSeconds(3);

        MudarEstadoJogador(true);
        Saci.GetComponent<Saci>().BatalhaBoss1();
        yield return new WaitUntil(() => Saci.GetComponent<Saci>().estaDerrotado);

        yield return new WaitForSeconds(2);
        MudarEstadoJogador(false);
        Debug.Log("derrotou saci");
        #endregion

        #region saci derrotado, andar em direção, dialogo
        yield return new WaitForSeconds(2);

        meninaAndando_depoisBossSaci = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_DepoisBossSaci.transform.position) < 1);
        meninaAndando_depoisBossSaci = false;

        Saci.transform.position = posSaci_DepoisBossSaci.position;
        yield return new WaitForSeconds(2);

        Interagir_Geral(Saci.GetComponent<SaciDialog>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        yield return new WaitForSeconds(2);
        Saci.SetActive(false);
        #endregion

        MudarEstadoJogador(true);
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

    void MovimentarGameObject(GameObject posInicial, Transform posNova, float velocidade)
    {
        posInicial.transform.position = Vector2.MoveTowards(posInicial.transform.position, posNova.position, velocidade * Time.deltaTime);
    }
}
