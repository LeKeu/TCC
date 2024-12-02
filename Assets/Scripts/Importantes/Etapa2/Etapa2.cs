using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Etapa2 : MonoBehaviour
{
    [SerializeField] GameObject Saci;

    #region geral
    [Header("Geral")]
    [SerializeField] SFX sfx_script;
    [SerializeField] Tutorial tutorial_script;
    //[SerializeField] ArmaAtiva armaAtiva;
    [SerializeField] OQueFazer oQueFazer_script;
    [SerializeField] InventarioAtivo inventarioAtivo;
    [SerializeField] LuzesCiclo luzesCiclo;
    [SerializeField] GameObject VidaUI;
    [SerializeField] GameObject dialogosGerais;
    #endregion

    #region Primeiro Encontro Saci
    [Header("Primeiro Encontro Saci")]
    [SerializeField] AudioClip SaciAssobio;
    [SerializeField] Transform posMenina_INICIO;
    [SerializeField] Transform posMenina_ConversaSaci;
    [SerializeField] Transform posMenina_AcabouLuta;
    [SerializeField] GameObject barreiraMoitasVermelhas;
    [SerializeField] GameObject particulasChuva;

    bool aconteceuEncontro;
    bool meninaAndando_EncontroSaci;
    bool meninaAndando_AcabouLuta;
    #endregion

    #region Boss Saci
    [Header("Boss Saci")]
    [SerializeField] Transform posMeninaINICIO;
    [SerializeField] Transform posMenina_BossSaci;
    [SerializeField] Transform posMenina_DepoisBossSaci;
    [SerializeField] Transform posSaci_DepoisBossSaci;
    //[SerializeField] GameObject barreiraEntrada;

    bool aconteceuBossSaci;
    bool meninaAndando_inicioBossSaci;
    bool meninaAndando_depoisBossSaci;
    #endregion

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "01_saci")
        {
            JogadorController.Instance.transform.position = posMenina_INICIO.position;
            //sfx_script.FlorestaNoite();
            StartCoroutine(ClarearTela(new Color(.12f, .16f, .5f)));
            JogadorController.Instance.velocidade = 1f;
            JogadorController.Instance.velInicial = 1f;
            Saci.SetActive(false);

            VidaUI.SetActive(false);
        }

        if(SceneManager.GetActiveScene().name == "03_saci") // MUDAR PARA CENA DO BOSS
        {
            StartCoroutine(ClarearTela(new Color(.12f, .16f, .5f)));
            JogadorController.Instance.transform.position = posMeninaINICIO.position;
        }
    }

    IEnumerator ClarearTela(Color cor)
    {
        #region fade de preto p branco
        MudarEstadoJogador(false);
        oQueFazer_script.AtivarPainelQuests(false);

        luzesCiclo.MudarCorAmbiente(Color.black);
        yield return new WaitForSeconds(2.5f);
        luzesCiclo.MudarCorAmbiente(cor, .3f);
        yield return new WaitForSeconds(5); // música tocando por x segundos

        oQueFazer_script.AtivarPainelQuests(true);
        MudarEstadoJogador(true);
        #endregion
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
        //Debug.Log("colidiu");
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
        oQueFazer_script.AtivarPainelQuests(false);

        #region  menina andando e escuta assobio
        meninaAndando_EncontroSaci = true;

        sfx_script.AssobioSaci();
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_ConversaSaci.transform.position) < 1);
        meninaAndando_EncontroSaci = false;
        MudarEstadoJogador(false);
        #endregion

        #region dialogo com ela mesma
        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 0, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        yield return new WaitForSeconds(2);
        #endregion

        #region saci aparecendo
        sfx_script.PararAssobioSaci();
        sfx_script.VentoniaSaci();
        yield return new WaitForSeconds(.5f);
        Saci.SetActive(true);
        yield return new WaitForSeconds(3);

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 1, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region parar chuva, dialogo
        sfx_script.VentoForteSaci();

        #region chuva part
        particulasChuva.GetComponent<ParticleSystem>().emissionRate = 150;
        yield return new WaitForSeconds(2);
        particulasChuva.GetComponent<ParticleSystem>().emissionRate = 75;
        yield return new WaitForSeconds(2);
        particulasChuva.GetComponent<ParticleSystem>().emissionRate = 30;
        yield return new WaitForSeconds(2);
        particulasChuva.GetComponent<ParticleSystem>().emissionRate = 0;
        #endregion

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 2, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        yield return new WaitForSeconds(1.5f);
        #endregion

        #region aparecer invocados
        Saci.GetComponent<Saci>().IniciarBatalha_primeiroEncontroSaci();
        InvocadoInimigo.podeAndar = false;
        yield return new WaitForSeconds(2);

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 3, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        InvocadoInimigo.podeAndar = true;
        #endregion

        #region ativar arma, tutorial dash e purificacao, inicio batalha saci
        sfx_script.MusicaCombate();
        VidaUI.SetActive(true);
        oQueFazer_script.AtivarPainelQuests(true);
        oQueFazer_script.GerenciarQuadroQuest_saci_cenas(1);

        ArmaAtiva.Instance.AtivarArma1(true);
        inventarioAtivo.AtivarArma1(true);

        JogadorController.Instance.velocidade = 3f;
        JogadorController.Instance.velInicial = 3f;

        tutorial_script.IniciarTutorial_PararTempo("Para dar um dash, aperte 'Barra de espaço'.", KeyCode.Space);
        MudarEstadoJogador(true);
        yield return new WaitUntil(() => InvocadoInimigo.podePurificar);
        tutorial_script.IniciarTutorial_PararTempo("Quando um inimigo estiver atordoado, chegue perto dele e aperte 'Q' para purificá-lo! ", KeyCode.Q);
        yield return new WaitUntil(() => !tutorial_script.duranteTutorial);

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("InvocadoInimigo").Length == 0);
        #endregion

        oQueFazer_script.AtivarPainelQuests(false);
        MudarEstadoJogador(false); // checar aqui!! o player ta andando estranho ás vezes

        #region acabou luta, dialogo saci
        sfx_script.PararMusicaCombate();
        yield return new WaitForSeconds(3);

        meninaAndando_AcabouLuta = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_AcabouLuta.transform.position) < 1);
        
        yield return new WaitForSeconds(3);

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 4, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        sfx_script.VentoniaSaci();
        yield return new WaitForSeconds(.5f);
        Saci.SetActive(false);

        barreiraMoitasVermelhas.SetActive(false);
        #endregion

        #region falando sozinha após o saci
        Debug.Log("falando sozinha");
        yield return new WaitForSeconds(1);
        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 5, "Interagir_CelebracaoCutscene");
        Debug.Log("acabou falando sozinha");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        yield return new WaitForSeconds(2);
        #endregion

        MudarEstadoJogador(true);

        oQueFazer_script.AtivarPainelQuests(true);
        oQueFazer_script.GerenciarQuadroQuest_saci_cenas(2);

        Etapas.PrimeiroEncontroSaci = false;
        JogadorController.Instance.ModificarVelocidade(4f);
    }

    IEnumerator BossSaci()
    {
        //barreiraEntrada.GetComponent<BoxCollider2D>().enabled = true;

        aconteceuBossSaci = true;
        Etapas.BossSaci = true;
        oQueFazer_script.AtivarPainelQuests(false);

        #region ir em direção do saci, dialogo começa
        meninaAndando_inicioBossSaci = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_BossSaci.transform.position) < 1);
        meninaAndando_inicioBossSaci = false;

        MudarEstadoJogador(false);
        yield return new WaitForSeconds(2); // middle button n ta passando dialogo p esse saciii

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 0, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region começar boss, esperar derrota saci
        sfx_script.MusicaCombate();
        yield return new WaitForSeconds(1.5f);

        MudarEstadoJogador(true);
        Saci.GetComponent<Saci>().BatalhaBoss1();
        yield return new WaitUntil(() => Saci.GetComponent<Saci>().estaDerrotado);

        MudarEstadoJogador(false);
        yield return new WaitForSeconds(2);
        #endregion

        #region saci derrotado, andar em direção, dialogo
        sfx_script.PararMusicaCombate();
        yield return new WaitForSeconds(2);

        meninaAndando_depoisBossSaci = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posMenina_DepoisBossSaci.transform.position) < 1);
        meninaAndando_depoisBossSaci = false;

        Saci.transform.position = posSaci_DepoisBossSaci.position;
        yield return new WaitForSeconds(2);

        Interagir_Geral(dialogosGerais.GetComponent<SeqCucaCelebracaoDialogos>(), 1, "Interagir_CelebracaoCutscene");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        yield return new WaitForSeconds(2);
        Saci.SetActive(false);
        #endregion

        //TEMPORÁRIO PARA DEMO
        luzesCiclo.MudarCorAmbiente(Color.black, 5f);
        yield return new WaitForSeconds(10);
        JogadorController.Instance.IrTelaDemo();
        //TEMPORÁRIO PARA DEMO

        oQueFazer_script.AtivarPainelQuests(true);
        oQueFazer_script.GerenciarQuadroQuest_saci_cenas(2);

        MudarEstadoJogador(true);
    }

    void Interagir_Geral(MonoBehaviour script, int index, string metodoNome = "Interagir")
    {
        object[] parametros = { index };
        var metodo = script.GetType().GetMethod($"{metodoNome}");
        metodo.Invoke(script, parametros);
    }

    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
        //JogadorController.Instance.podeFlipX = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
    }

    void MovimentarGameObject(GameObject posInicial, Transform posNova, float velocidade)
    {
        posInicial.transform.position = Vector2.MoveTowards(posInicial.transform.position, posNova.position, velocidade * Time.deltaTime);
    }
}
