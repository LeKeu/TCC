using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Prologo : MonoBehaviour
{
    [SerializeField] AudioClip musicaMenina;
    AudioSource musicaSource;
    Menino meninoScript;

    public static int qntdNpcsConversados = 0;
    int totalNpcsConversaveis = 1;

    #region bool de checagem de etapas ocorridas
    static bool aconteceuBriga;
    bool aconteceuSequestro;
    #endregion

    #region NPCS GO
    GameObject velhaNamiaCelebracaoGO;
    GameObject meninoGO;
    GameObject seuPedroGO;
    #endregion

    #region BRIGA
    [Header("Briga Menino")]
    [SerializeField] GameObject posMeninaBriga;
    [SerializeField] GameObject posMeninoBriga;
    [SerializeField] GameObject posVelhaNamiaBriga;
    [SerializeField] GameObject posSeuPedroBriga;
    [SerializeField] GameObject posMeninoSaindo;

    bool pessoasAndando;
    bool meninoPodeSair;
    #endregion

    #region SEQUESTRO
    [Header("Sequestro Menino")]
    [SerializeField] GameObject posFrenteCasa;
    [SerializeField] GameObject vulto;

    [SerializeField] AudioClip musicaCuca;
    [SerializeField] AudioClip vidroEstoura;
    [SerializeField] AudioClip passosVidro;

    AudioSource audioSourceSequestro;
    bool meninaIndoCasa;
    bool vultoMovendo;
    #endregion

    JogadorController jogadorController;
    TremerCamera tremerCamera;

    private void Awake()
    {
        musicaSource = GetComponent<AudioSource>();
        meninoScript = GameObject.Find("Menino").GetComponent<Menino>();
        jogadorController = JogadorController.Instance;
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
    }

    private void Start()
    {
        Debug.Log(jogadorController.velocidade + " velllll");
        velhaNamiaCelebracaoGO = GameObject.Find("VelhaNamia");
        meninoGO = GameObject.Find("Menino");
        seuPedroGO = GameObject.Find("SeuPedro");

        if (SceneManager.GetActiveScene().name == "01_comunidade")
            StartCoroutine(IniciarJogo_MeninaTocando());

        if(this.gameObject.name == "TriggerCucaSeq")
        {
            audioSourceSequestro = GetComponent<AudioSource>();
        }
    }

    private void FixedUpdate()
    {
        #region Briga
        if (Etapas.BrigaCelebracao && this.gameObject.name == "TriggerBriga")
        {
            if (pessoasAndando) // movimenetar os personagens para a área de celebração
            {
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMeninaBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
                velhaNamiaCelebracaoGO.transform.position = Vector2.MoveTowards(velhaNamiaCelebracaoGO.transform.position, posVelhaNamiaBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
                meninoGO.transform.position = Vector2.MoveTowards(meninoGO.transform.position, posMeninoBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
                seuPedroGO.transform.position = Vector2.MoveTowards(seuPedroGO.transform.position, posSeuPedroBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
                
            }

            if(meninoPodeSair)
                meninoGO.transform.position = Vector2.MoveTowards(meninoGO.transform.position, posMeninoSaindo.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
        }
        #endregion

        #region Sequestro
        if (Etapas.CucaSequestro && this.gameObject.name == "TriggerCucaSeq")
        {
            if (meninaIndoCasa)
            {
                jogadorController.transform.position = Vector2.MoveTowards(jogadorController.transform.position, posFrenteCasa.transform.position, jogadorController.velocidade * Time.deltaTime);
            }

            if (vultoMovendo)
                vulto.transform.Translate(Vector2.right * 8f * Time.deltaTime);
        }
        #endregion

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("TRIGGER");
        //Debug.Log(gameObject.name);
        //Debug.Log(aconteceuBriga);

        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerBriga" && qntdNpcsConversados >= totalNpcsConversaveis && !aconteceuBriga)
            StartCoroutine(Brigar());
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerCucaSeq"/* && aconteceuBriga*/)
            StartCoroutine(CucaSequestraMenino());
        //if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerPerseguirCuca")
        //    PerseguirCuca();
    } 

    void Interagir_Celebracao(MonoBehaviour script, int index)
    {
        object[] parametros = { index };
        //Debug.Log(script.name);
        var metodo = script.GetType().GetMethod($"Interagir_CelebracaoCutscene");
        metodo.Invoke(script, parametros);
    }

    IEnumerator IniciarJogo_MeninaTocando()
    {
        Etapas.MeninaTocandoUkulele = true;

        MudarEstadoJogador(false);

        musicaSource.PlayOneShot(musicaMenina);

        yield return new WaitForSeconds(10); // música tocando por x segundos

        meninoScript.podeMover = true;
        musicaSource.Stop();

        yield return new WaitUntil(() => !meninoScript.estaLonge); // esperar até o menino chegar perto
        meninoScript.Interagir();

        yield return new WaitUntil(() => jogadorController.acabouDialogo); // esperar dialogo com menino acabar

        MudarEstadoJogador(true);
        Menino.acabouFalar = true;

        Etapas.MeninaTocandoUkulele = false;
    }

    IEnumerator Brigar()
    {
        Debug.Log("BRIGARRR");
        Etapas.BrigaCelebracao = true;

        #region Povo se movendo na direção de sua pos da cena
        pessoasAndando = true;
        yield return new WaitForSeconds(3);
        pessoasAndando = false;
        #endregion

        MudarEstadoJogador(false);

        #region Dialogos Celebraçao
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 1);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);
        #endregion
        yield return new WaitForSeconds(2);
        #region Dialogo Briga
        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 1);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);
        #endregion

        #region Menino indo embora
        meninoPodeSair = true;
        yield return new WaitForSeconds(3);
        meninoPodeSair = false;
        //meninoGO.SetActive(false); // por algum motivo, desativar ele desativa tbm a continuação do dialogo
        #endregion
        yield return new WaitForSeconds(2);
        #region Dialogos Pos Briga
        Interagir_Celebracao(seuPedroGO.GetComponent<SeuPedroCelebracao>(), 1);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 2);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);
        #endregion

        MudarEstadoJogador(true);

        Etapas.BrigaCelebracao = false;
        aconteceuBriga = true;
    }

    IEnumerator CucaSequestraMenino()
    {
        Debug.Log("SEQUESTROW");

        MudarEstadoJogador(false);
        Etapas.CucaSequestro = true;

        #region Menina andando em direção da casa
        meninaIndoCasa = true;
        //yield return new WaitUntil(() => new Vector2(jogadorController.transform.position.x, jogadorController.transform.position.y) == new Vector2(posFrenteCasa.transform.position.x, posFrenteCasa.transform.position.y) ? true : false);
        yield return new WaitForSeconds(3);
        meninaIndoCasa = false;
        #endregion

        #region musica cuca e dialogo menina
        audioSourceSequestro.PlayOneShot(musicaCuca);
        audioSourceSequestro.loop = true;
        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 2);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);
        #endregion

        #region Barulhos vidros, tremer cam, pedido socorro
        audioSourceSequestro.loop = false;
        audioSourceSequestro.Stop();

        yield return new WaitForSeconds(.5f);
        audioSourceSequestro.PlayOneShot(vidroEstoura);
        tremerCamera.TremerCameraFuncDinamica(3f, 2f);
        yield return new WaitForSeconds(vidroEstoura.length);

        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 3);
        yield return new WaitUntil(() => jogadorController.acabouDialogo);

        audioSourceSequestro.PlayOneShot(passosVidro);
        #endregion

        #region Vulto movendo
        vultoMovendo = true;
        vulto.SetActive(true);
        yield return new WaitForSeconds(2);
        vulto.SetActive(false);
        vultoMovendo = false;
        #endregion
        Etapas.CucaSequestro = false;
    }

    //void PerseguirCuca()
    //{
    //    Debug.Log("perseguir");
    //}

    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
    }
}
