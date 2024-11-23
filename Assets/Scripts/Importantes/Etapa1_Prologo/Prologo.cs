using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Metadata;

public class Prologo : MonoBehaviour
{
    [Header("Gerais")]
    [SerializeField] OQueFazer oQueFazer_script;
    CinemachineVirtualCamera virtualCamera;
    TremerCamera tremerCamera;
    LuzesCiclo luzesCiclo;

    [Header("Audios Geral")]
    [SerializeField] SFX sfx_script;
    AudioSource musicaSource;
    Menino meninoScript;

    public static int qntdNpcsConversados = 0;
    int totalNpcsConversaveis = 1;

    #region bool de checagem de etapas ocorridas
    public static bool aconteceuBriga;
    bool mudouTextAux;
    #endregion

    #region NPCS GO
    [Header("NPCS")]
    [SerializeField] GameObject Npcs;
    List<GameObject> NpcsLista = new List<GameObject>();

    GameObject donaMartaGO;
    GameObject eloaGO;
    GameObject ladraoGO;
    GameObject pedrinhoGO;
    GameObject seuJoaoGO;
    GameObject seuPedroGO;
    GameObject velhoDoidoGO;
    GameObject velhaNamiaCelebracaoGO;
    GameObject meninoGO;
    #endregion

    #region UKULELE
    [SerializeField] AudioClip musicaMenina;
    #endregion

    #region BRIGA
    [Header("Celebra��o - Briga Menino")]
    [SerializeField] GameObject posicoesNpcs;
    [SerializeField] GameObject posCamera;
    List<GameObject> posicoesNpcsLista = new List<GameObject>();

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
    [SerializeField] AudioClip garraCuca;
    [SerializeField] AudioClip meninaCorpoCaindo;

    [SerializeField] List<GameObject> moitasVermelhas;
    [SerializeField] List<GameObject> listaPosMeninaCorrendo;

    AudioSource audioSourceSequestro;
    bool meninaIndoCasa;
    bool vultoMovendo;

    bool meninaCorrendo;
    GameObject proxPosMeninaAux;
    bool podeIrProxPos;
    #endregion

    #region SOZINHA FLORESTA
    [Header("FLORESTA SOZINHA")]
    [SerializeField] GameObject DialogoBox;
    [SerializeField] Transform posInicialMenina;
    #endregion

    private void Awake()
    {
        musicaSource = GetComponent<AudioSource>();
        meninoScript = GameObject.Find("Bernardo").GetComponent<Menino>();
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();

        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        tremerCamera = virtualCamera.GetComponent<TremerCamera>();

        if(this.gameObject.name == "TriggerBriga")
        { // s� pega os npcs nas cenas com comunidade
            foreach (Transform child in Npcs.transform) // pegando GO de cada NPC
                NpcsLista.Add(child.gameObject);
        }

        if(this.gameObject.name == "TriggerBriga")
        {
            foreach (Transform child in posicoesNpcs.transform) // posi��es durante celebra��o
                posicoesNpcsLista.Add(child.gameObject);

            #region Npcs GO
            donaMartaGO = NpcsLista[0];
            eloaGO = NpcsLista[1];
            ladraoGO = NpcsLista[2];
            pedrinhoGO = NpcsLista[3];
            seuJoaoGO = NpcsLista[4];
            seuPedroGO = NpcsLista[5];
            velhoDoidoGO = NpcsLista[6];
            velhaNamiaCelebracaoGO = NpcsLista[7];
            meninoGO = NpcsLista[8];
            #endregion
        }

        if (this.gameObject.name == "TriggerCucaSeq")
            audioSourceSequestro = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "01_comunidade")
            StartCoroutine(IniciarJogo_MeninaTocando());

        if (SceneManager.GetActiveScene().name == "02_comunidade")
            oQueFazer_script.GerenciarQuadroQuest_celebr_seq(0);

        if (SceneManager.GetActiveScene().name == "T03_comunidade" && this.gameObject.name == "FlorestaSozinha")
            StartCoroutine(Iniciar_FlorestaEscura());
    }

    private void FixedUpdate()
    {
        if(qntdNpcsConversados >= totalNpcsConversaveis  && !mudouTextAux)
        {
            oQueFazer_script.GerenciarQuadroQuest_celebr_seq(1);
            mudouTextAux = true;
        }

        #region Briga
        if (Etapas.BrigaCelebracao && this.gameObject.name == "TriggerBriga")
        {
            if (pessoasAndando) // movimentar os personagens para a �rea de celebra��o
            {
                MovimentarPessoas();
            }

            if(meninoPodeSair)
                meninoGO.transform.position = Vector2.MoveTowards(meninoGO.transform.position, posicoesNpcsLista[10].transform.position, 4.5f * Time.deltaTime);
        }
        #endregion

        #region Sequestro
        if (Etapas.CucaSequestro && this.gameObject.name == "TriggerCucaSeq")
        {
            if (meninaIndoCasa)
            {
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posFrenteCasa.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
            }

            if (vultoMovendo)
                vulto.transform.Translate(Vector2.right * 8f * Time.deltaTime);

            if (meninaCorrendo)
            {
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, proxPosMeninaAux.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
                if (Vector3.Distance(JogadorController.Instance.transform.position, proxPosMeninaAux.transform.position) <= 1f)
                    podeIrProxPos = true;
            }
        }
        #endregion

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerBriga" && qntdNpcsConversados >= totalNpcsConversaveis && !aconteceuBriga)
            StartCoroutine(Brigar());
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerCucaSeq" /*&& aconteceuBriga*/)
            StartCoroutine(CucaSequestraMenino());
    } 

    void MovimentarPessoas()
    {
        JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posicoesNpcsLista[9].transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
        donaMartaGO.transform.position = Vector2.MoveTowards(donaMartaGO.transform.position, posicoesNpcsLista[0].transform.position, 2 * Time.deltaTime);
        eloaGO.transform.position = Vector2.MoveTowards(eloaGO.transform.position, posicoesNpcsLista[1].transform.position, 2.5f * Time.deltaTime);
        ladraoGO.transform.position = Vector2.MoveTowards(ladraoGO.transform.position, posicoesNpcsLista[2].transform.position, 2 * Time.deltaTime);
        pedrinhoGO.transform.position = Vector2.MoveTowards(pedrinhoGO.transform.position, posicoesNpcsLista[3].transform.position, 2.5f * Time.deltaTime);
        seuJoaoGO.transform.position = Vector2.MoveTowards(seuJoaoGO.transform.position, posicoesNpcsLista[4].transform.position, 1.5f * Time.deltaTime);
        seuPedroGO.transform.position = Vector2.MoveTowards(seuPedroGO.transform.position, posicoesNpcsLista[5].transform.position, 2 * Time.deltaTime);
        velhoDoidoGO.transform.position = Vector2.MoveTowards(velhoDoidoGO.transform.position, posicoesNpcsLista[6].transform.position, 1f * Time.deltaTime);
        velhaNamiaCelebracaoGO.transform.position = Vector2.MoveTowards(velhaNamiaCelebracaoGO.transform.position, posicoesNpcsLista[7].transform.position, .5f * Time.deltaTime);
        meninoGO.transform.position = Vector2.MoveTowards(meninoGO.transform.position, posicoesNpcsLista[8].transform.position, 3f * Time.deltaTime);
    }

    void Interagir_Celebracao(MonoBehaviour script, int index, string metodoNome = "Interagir_CelebracaoCutscene")
    {
        object[] parametros = { index };
        //Debug.Log(script.name);
        var metodo = script.GetType().GetMethod($"{metodoNome}");
        metodo.Invoke(script, parametros);
    }

    IEnumerator IniciarJogo_MeninaTocando()
    {
        Etapas.MeninaTocandoUkulele = true;
        oQueFazer_script.AtivarPainelQuests(false);

        MudarEstadoJogador(false);

        #region Tocar ukulele
        musicaSource.PlayOneShot(musicaMenina);

        yield return new WaitForSeconds(3); // m�sica tocando por x segundos
        #endregion

        #region menino chega, dialogo
        meninoScript.podeMover = true;

        yield return new WaitUntil(() => !meninoScript.estaLonge); // esperar at� o menino chegar perto
        meninoScript.Interagir();
        musicaSource.Stop();

        Debug.Log("para msuica LET");
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo); // esperar dialogo com menino acabar
        #endregion

        MudarEstadoJogador(true);
        Menino.acabouFalar = true;

        Etapas.MeninaTocandoUkulele = false;
        oQueFazer_script.AtivarPainelQuests(true);
    }

    IEnumerator Brigar()
    {
        Etapas.BrigaCelebracao = true;
        virtualCamera.Follow = posCamera.transform;
        oQueFazer_script.AtivarPainelQuests(false);

        #region Povo se movendo na dire��o de sua pos da cena
        pessoasAndando = true;
        yield return new WaitUntil(() => Vector2.Distance(velhaNamiaCelebracaoGO.transform.position, posicoesNpcsLista[7].transform.position) < 0.1f);
        pessoasAndando = false;
        #endregion

        MudarEstadoJogador(false);

        #region Dialogos Celebra�ao
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        yield return new WaitForSeconds(2);

        #region Dialogo Briga
        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region Menino indo embora
        meninoPodeSair = true;
        yield return new WaitForSeconds(3);
        meninoPodeSair = false;
        //meninoGO.SetActive(false); // por algum motivo, desativar ele desativa tbm a continua��o do dialogo
        #endregion

        yield return new WaitForSeconds(2);

        #region Dialogos Pos Briga
        Interagir_Celebracao(seuPedroGO.GetComponent<SeuPedroCelebracao>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 2);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region tudo preto, dialogue, volta de noite
        luzesCiclo.MudarCorAmbiente(Color.black, 5f);
        yield return new WaitForSeconds(10);

        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 3);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        yield return new WaitForSeconds(2);
        luzesCiclo.MudarCorAmbiente(new Color(0.19f, .2f, 1), 5f);
        #endregion

        #region Dialogos finalizando celebra��o
        yield return new WaitForSeconds(4);
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 4);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        MudarEstadoJogador(true);
        oQueFazer_script.AtivarPainelQuests(true);
        oQueFazer_script.GerenciarQuadroQuest_celebr_seq(2);

        virtualCamera.Follow = JogadorController.Instance.transform;
        Etapas.BrigaCelebracao = false;
        aconteceuBriga = true;
    }

    IEnumerator CucaSequestraMenino()
    {
        Debug.Log("SEQUESTROW");

        Etapas.CucaSequestro = true;

        #region Menina andando em dire��o da casa
        meninaIndoCasa = true;
        //yield return new WaitUntil(() => new Vector2(jogadorController.transform.position.x, jogadorController.transform.position.y) == new Vector2(posFrenteCasa.transform.position.x, posFrenteCasa.transform.position.y) ? true : false);
        yield return new WaitForSeconds(3);
        meninaIndoCasa = false;
        #endregion

        MudarEstadoJogador(false);
        oQueFazer_script.AtivarPainelQuests(false);

        #region musica cuca e dialogo menina
        audioSourceSequestro.PlayOneShot(musicaCuca);
        audioSourceSequestro.loop = true;
        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 2);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region Barulhos vidros, tremer cam, pedido socorro
        audioSourceSequestro.loop = false;
        audioSourceSequestro.Stop();

        yield return new WaitForSeconds(.5f);
        audioSourceSequestro.PlayOneShot(vidroEstoura);
        tremerCamera.TremerCameraFuncDinamica(2f, 2f);
        yield return new WaitForSeconds(vidroEstoura.length);

        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 3);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        audioSourceSequestro.PlayOneShot(passosVidro);
        #endregion

        #region Vulto movendo, moitas sumindo
        vulto.SetActive(true);
        vultoMovendo = true;
        yield return new WaitForSeconds(2);
        vulto.SetActive(false);
        vultoMovendo = false;

        foreach(GameObject moita in moitasVermelhas)
            moita.SetActive(false);
        #endregion

        #region Menina Correndo
        meninaCorrendo = true;
        foreach (GameObject pos in listaPosMeninaCorrendo)
        {
            proxPosMeninaAux = pos;
            yield return new WaitUntil(() => podeIrProxPos);
            podeIrProxPos = false;
        }
        meninaCorrendo = false;

        #endregion

        //efeitos sonoros tensos, tela escurece r�pido, efeito sonoro de garra, volta sprite menina ca�do, bem mais de noite, menina segue caminho
        #region Tela escurecer, barulho garra, muda sprite, volta bem escuro, 
        luzesCiclo.MudarCorAmbiente(Color.black);
        yield return new WaitForSeconds(.5f);
        audioSourceSequestro.PlayOneShot(garraCuca);
        yield return new WaitForSeconds(garraCuca.length);
        audioSourceSequestro.PlayOneShot(meninaCorpoCaindo);
        yield return new WaitForSeconds(3);
        //luzesCiclo.MudarCorAmbiente(new Color(0.05f, .07f, .21f), 4f);
        #endregion

        Etapas.CucaSequestro = false;
        SceneManager.LoadScene("T03_comunidade");
    }

    IEnumerator Iniciar_FlorestaEscura()
    {
        JogadorController.Instance.transform.position = posInicialMenina.position;
        //oQueFazer_script.AtivarPainelQuests(false);
        oQueFazer_script.GerenciarQuadroQuest_celebr_seq(3);

        sfx_script.FlorestaNoite();
        JogadorController.Instance.ModificarVelocidade(1f);

        #region fade from black
        MudarEstadoJogador(false);
        luzesCiclo.MudarCorAmbiente(Color.black);
        luzesCiclo.MudarCorAmbiente(new Color(0.05f, .07f, .21f), 4f);
        yield return new WaitForSeconds(5);
        MudarEstadoJogador(true);
        #endregion

        #region dialogos menina
        JogadorController.Instance.falandoSozinha = true;
        JogadorDialogo.Instance.Interagir_CelebracaoCutscene();
        yield return new WaitForSeconds(5);
        JogadorDialogo.Instance.Interagir_CelebracaoCutscene();
        yield return new WaitForSeconds(5);
        JogadorDialogo.Instance.Interagir_CelebracaoCutscene();
        yield return new WaitForSeconds(5);
        JogadorDialogo.Instance.Interagir_CelebracaoCutscene();
        yield return new WaitForSeconds(5);
        DialogoBox.SetActive(false);
        #endregion

        //oQueFazer_script.AtivarPainelQuests(true);
        JogadorController.Instance.falandoSozinha = false;
        JogadorController.Instance.estaDuranteCutscene = false;
        JogadorController.Instance.acabouDialogo = true;
    }

    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
        //JogadorController.Instance.podeFlipX = !acao;
    }
}
