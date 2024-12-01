using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Prologo : MonoBehaviour
{
    [Header("Gerais")]
    [SerializeField] OQueFazer oQueFazer_script;
    [SerializeField] DialogosGerais dialogosGerais;
    CinemachineVirtualCamera virtualCamera;
    TremerCamera tremerCamera;
    AjusteTamanhoCamera ajusteTamanhoCamera;
    LuzesCiclo luzesCiclo;

    [Header("Audios Geral")]
    [SerializeField] SFX sfx_script;
    AudioSource musicaSource;
    Menino meninoScript;

    public static int qntdNpcsConversados = 0;
    int totalNpcsConversaveis = 1;

    #region bool de checagem de etapas ocorridas
    public static bool aconteceuBriga;
    public static bool procurandoBola;
    bool mudouTextAux;
    bool finalizouQuests;
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
    [Header("Celebração - Briga Menino")]
    [SerializeField] GameObject posicoesNpcs;
    [SerializeField] GameObject posCamera;
    List<GameObject> posicoesNpcsLista = new List<GameObject>();

    bool pessoasAndando;
    bool meninoPodeSair;
    #endregion

    #region SEQUESTRO
    [Header("Sequestro Menino")]
    [SerializeField] GameObject dialogosGeraisSeq;
    [SerializeField] GameObject posFrenteCasa;
    [SerializeField] GameObject vulto;
    [SerializeField] GameObject chuvaParticulas;

    [SerializeField] List<GameObject> posVelhaNamiaSeqCuca;
    [SerializeField] List<GameObject> moitasVermelhas;
    [SerializeField] List<GameObject> listaPosMeninaCorrendo;

    AudioSource audioSourceSequestro;
    bool meninaIndoCasa;
    bool vultoMovendo;

    GameObject proxPosMeninaAux;
    bool meninaCorrendo;
    bool podeIrProxPos;
    bool velhaNamiaAuxBool;
    int velhaNamiaAuxInt;
    #endregion

    #region SOZINHA FLORESTA
    [Header("FLORESTA SOZINHA")]
    [SerializeField] GameObject DialogoBox;
    [SerializeField] Transform posInicialMenina;
    #endregion

    private void Awake()
    {
        FinalizarQuests.todosTutCompleto = false; // iniciar aqui ou fica num loop repetindo a cena
        musicaSource = GetComponent<AudioSource>();
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();

        if(SceneManager.GetActiveScene().name != "T03_comunidade")
        {
            meninoScript = GameObject.Find("Bernardo").GetComponent<Menino>(); // NÃO É NECESSÁRIO

            foreach (Transform child in Npcs.transform) // pegando GO de cada NPC
                NpcsLista.Add(child.gameObject);

            if (this.gameObject.name == "TriggerBriga")
            {
                foreach (Transform child in posicoesNpcs.transform) // posições durante celebração
                    posicoesNpcsLista.Add(child.gameObject);
            }
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

        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        tremerCamera = virtualCamera.GetComponent<TremerCamera>();
        ajusteTamanhoCamera = virtualCamera.GetComponent<AjusteTamanhoCamera>();

        if (this.gameObject.name == "TriggerCucaSeq")
            audioSourceSequestro = GetComponent<AudioSource>();
    }

    private void Start()
    {
        qntdNpcsConversados = 0;

        if (SceneManager.GetActiveScene().name == "01_comunidade")
        {
            StartCoroutine(IniciarJogo_MeninaTocando());
            //sfx_script.ComunidadeFloresta();
        }

        if (SceneManager.GetActiveScene().name == "02_comunidade")
        {
            oQueFazer_script.GerenciarQuadroQuest_celebr_seq(0);
            //sfx_script.ComunidadeFloresta();

            StartCoroutine(ClarearTela(new Color(1, .41f, .41f)));
        }

        if (SceneManager.GetActiveScene().name == "T03_comunidade" && this.gameObject.name == "FlorestaSozinha")
            StartCoroutine(Iniciar_FlorestaEscura());
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

    IEnumerator EscurecerTela(float segundos = 5)
    {
        #region fade de preto p branco
        MudarEstadoJogador(false);
        oQueFazer_script.AtivarPainelQuests(false);

        luzesCiclo.MudarCorAmbiente(Color.black, .8f);
        yield return new WaitForSeconds(segundos);
        #endregion
        SceneManager.LoadScene("02_comunidade");
    }

    private void FixedUpdate()
    {
        if (FinalizarQuests.todosTutCompleto && !finalizouQuests)
            CompletarQuests();

        if(qntdNpcsConversados >= totalNpcsConversaveis  && !mudouTextAux)
        {
            StartCoroutine(IniciarProcurarBola());
        }

        #region Briga
        if (Etapas.BrigaCelebracao && this.gameObject.name == "TriggerBriga")
        {
            if (pessoasAndando) // movimentar os personagens para a área de celebração
            {
                MovimentarPessoas();
            }

            if(meninoPodeSair)
                meninoGO.transform.position = Vector2.MoveTowards(meninoGO.transform.position, posicoesNpcsLista[10].transform.position, 3 * Time.deltaTime);
        }
        #endregion

        #region Sequestro
        if (Etapas.CucaSequestro && this.gameObject.name == "TriggerCucaSeq")
        {
            if (meninaIndoCasa)
            {
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posFrenteCasa.transform.position, 1 * Time.deltaTime);
            }

            if (vultoMovendo)
                vulto.transform.Translate(Vector2.right * 8f * Time.deltaTime);

            if (meninaCorrendo)
            {
                JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, proxPosMeninaAux.transform.position, 4.5f * Time.deltaTime);
                if (Vector3.Distance(JogadorController.Instance.transform.position, proxPosMeninaAux.transform.position) <= 1f)
                    podeIrProxPos = true;
            }

            if(velhaNamiaAuxBool)
                velhaNamiaCelebracaoGO.transform.position = Vector2.MoveTowards(velhaNamiaCelebracaoGO.transform.position, posVelhaNamiaSeqCuca[velhaNamiaAuxInt].transform.position, 1f * Time.deltaTime);
        }
        #endregion

    }

    IEnumerator IniciarProcurarBola()
    { // quando tiver ok para começar a celebração, faz isso
        mudouTextAux = true;
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        procurandoBola = true;

        JogadorController.Instance.podeFalar = false;

        yield return new WaitForSeconds(5);

        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        Interagir_Celebracao(dialogosGerais.GetComponent<DialogosGerais>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        JogadorController.Instance.podeFalar = true;
        oQueFazer_script.GerenciarQuadroQuest_celebr_seq(1);
        procurandoBola = false;
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
        donaMartaGO.transform.position = posicoesNpcsLista[0].transform.position;
        eloaGO.transform.position = posicoesNpcsLista[1].transform.position;
        ladraoGO.transform.position = posicoesNpcsLista[2].transform.position;
        pedrinhoGO.transform.position = posicoesNpcsLista[3].transform.position;
        seuJoaoGO.transform.position = posicoesNpcsLista[4].transform.position;
        seuPedroGO.transform.position = posicoesNpcsLista[5].transform.position;
        velhoDoidoGO.transform.position = posicoesNpcsLista[6].transform.position;
        velhaNamiaCelebracaoGO.transform.position = posicoesNpcsLista[7].transform.position;

        JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posicoesNpcsLista[9].transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
        //donaMartaGO.transform.position = Vector2.MoveTowards(donaMartaGO.transform.position, posicoesNpcsLista[0].transform.position, 2 * Time.deltaTime);
        //eloaGO.transform.position = Vector2.MoveTowards(eloaGO.transform.position, posicoesNpcsLista[1].transform.position, 2.5f * Time.deltaTime);
        //ladraoGO.transform.position = Vector2.MoveTowards(ladraoGO.transform.position, posicoesNpcsLista[2].transform.position, 2 * Time.deltaTime);
        //pedrinhoGO.transform.position = Vector2.MoveTowards(pedrinhoGO.transform.position, posicoesNpcsLista[3].transform.position, 2.5f * Time.deltaTime);
        //seuJoaoGO.transform.position = Vector2.MoveTowards(seuJoaoGO.transform.position, posicoesNpcsLista[4].transform.position, 1.5f * Time.deltaTime);
        //seuPedroGO.transform.position = Vector2.MoveTowards(seuPedroGO.transform.position, posicoesNpcsLista[5].transform.position, 2 * Time.deltaTime);
        //velhoDoidoGO.transform.position = Vector2.MoveTowards(velhoDoidoGO.transform.position, posicoesNpcsLista[6].transform.position, 1f * Time.deltaTime);
        //velhaNamiaCelebracaoGO.transform.position = Vector2.MoveTowards(velhaNamiaCelebracaoGO.transform.position, posicoesNpcsLista[7].transform.position, .5f * Time.deltaTime);
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
        #region Tocar ukulele
        //sfx_script.PararAudioSource03();
        oQueFazer_script.AtivarPainelQuests(false);
        #endregion

        #region fade de preto p branco
        MudarEstadoJogador(false);
        luzesCiclo.MudarCorAmbiente(Color.black);
        yield return new WaitForSeconds(3);
        musicaSource.PlayOneShot(musicaMenina);
        yield return new WaitForSeconds(5f);
        luzesCiclo.MudarCorAmbiente(Color.white, .2f);
        yield return new WaitForSeconds(10); // música tocando por x segundos
        #endregion

        Etapas.MeninaTocandoUkulele = true;

        MudarEstadoJogador(false);

        #region menino chega, dialogo
        meninoScript.podeMover = true;
        yield return new WaitUntil(() => !meninoScript.estaLonge); // esperar até o menino chegar perto

        musicaSource.Stop();
        yield return new WaitForSeconds(2);

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo); // esperar dialogo com menino acabar
        #endregion

        MudarEstadoJogador(true);
        Menino.acabouFalar = true;

        Etapas.MeninaTocandoUkulele = false;
        oQueFazer_script.AtivarPainelQuests(true);
        sfx_script.TocarAudioSource03();
    }

    void CompletarQuests()
    {
        finalizouQuests = true;
        StartCoroutine(EscurecerTela());
    }

    IEnumerator Brigar()
    {
        Etapas.BrigaCelebracao = true;
        oQueFazer_script.AtivarPainelQuests(false);

        #region parando, escutando o sino, dialogo menina bernardo
        MudarEstadoJogador(false);
        yield return new WaitForSeconds(1);
        sfx_script.ComunidadeSino();
        yield return new WaitForSeconds(3);

        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 4);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        sfx_script.ComunidadeSino();
        yield return new WaitForSeconds(3);

        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 5);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        MudarEstadoJogador(true);
        #endregion

        #region Povo se movendo na direção de sua pos da cena
        pessoasAndando = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posicoesNpcsLista[9].transform.position) < 0.1f);
        pessoasAndando = false;
        yield return new WaitForSeconds(2);
        #endregion

        ajusteTamanhoCamera.AjustarTamanhoCamera(3, 30);
        virtualCamera.Follow = posCamera.transform;
        yield return new WaitForSeconds(2);
        MudarEstadoJogador(false);

        #region Dialogos Celebraçao
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 2);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        yield return new WaitForSeconds(2);

        #region Dialogo Briga
        virtualCamera.Follow = JogadorController.Instance.transform;
        ajusteTamanhoCamera.AjustarTamanhoCamera(1.5f, 30);

        Interagir_Celebracao(meninoGO.GetComponent<Menino>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region Menino indo embora
        meninoPodeSair = true;
        yield return new WaitForSeconds(3);
        meninoPodeSair = false;
        meninoGO.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //meninoGO.SetActive(false); // por algum motivo, desativar ele desativa tbm a continuação do dialogo
        #endregion

        yield return new WaitForSeconds(2);

        virtualCamera.Follow = posCamera.transform;
        ajusteTamanhoCamera.AjustarTamanhoCamera(tempo:30);
        #region Dialogos Pos Briga
        Interagir_Celebracao(seuPedroGO.GetComponent<NPCsGERAL>(), 4);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 3);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region tudo preto, dialogue, volta de noite
        luzesCiclo.MudarCorAmbiente(Color.black, 5f);
        yield return new WaitForSeconds(5);

        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 4);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        yield return new WaitForSeconds(2);
        luzesCiclo.MudarCorAmbiente(new Color(0.19f, .2f, 1), 5f);
        #endregion

        #region Dialogos finalizando celebração
        yield return new WaitForSeconds(4);
        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 5);
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
        sfx_script.PararAudioSource03();
        chuvaParticulas.SetActive(true);

        Etapas.CucaSequestro = true;
        oQueFazer_script.AtivarPainelQuests(false);

        #region Velha namia falando de longe
        velhaNamiaCelebracaoGO.transform.position = posVelhaNamiaSeqCuca[0].transform.position;

        MudarEstadoJogador(false);
        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 0);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region velha namia conversando com menina e indo embora
        velhaNamiaAuxInt = 1;
        velhaNamiaAuxBool = true;
        yield return new WaitUntil(() => Vector2.Distance(velhaNamiaCelebracaoGO.transform.position, posVelhaNamiaSeqCuca[1].transform.position) < 0.1f);
        velhaNamiaAuxBool = false;

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 1);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        yield return new WaitForSeconds(2);

        velhaNamiaAuxInt = 0;
        velhaNamiaAuxBool = true;
        yield return new WaitUntil(() => Vector2.Distance(velhaNamiaCelebracaoGO.transform.position, posVelhaNamiaSeqCuca[0].transform.position) < 0.1f);
        velhaNamiaAuxBool = false;
        #endregion

        #region menina falando sozinha
        yield return new WaitForSeconds(2);
        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 2);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region Menina andando em direção da casa, musica tocando
        meninaIndoCasa = true;
        sfx_script.MusicaCuca();
        audioSourceSequestro.loop = true;
        yield return new WaitUntil(() => Vector2.Distance(JogadorController.Instance.transform.position, posFrenteCasa.transform.position) < 0.1f);
        meninaIndoCasa = false;

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 3);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        MudarEstadoJogador(false);

        //#region dialogo menina
        //Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 2);
        //yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        //#endregion

        audioSourceSequestro.loop = false;
        audioSourceSequestro.Stop();

        #region barulho coisa caindo, dialogo menina
        sfx_script.CoisasCaindo();
        yield return new WaitForSeconds(1.5f);

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 4);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        #endregion

        #region barulho de vidro, dialogo
        yield return new WaitForSeconds(.5f);
        sfx_script.VidroEstora();
        sfx_script.PararMusicaCuca();
        tremerCamera.TremerCameraFuncDinamica(2f, 1f);
        yield return new WaitForSeconds(1);

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 5);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        sfx_script.PassosVidro();

        #endregion

        #region Vulto movendo, moitas sumindo
        vulto.SetActive(true);
        vultoMovendo = true;
        yield return new WaitForSeconds(2);
        vulto.SetActive(false);
        vultoMovendo = false;

        Interagir_Celebracao(dialogosGeraisSeq.GetComponent<SeqCucaCelebracaoDialogos>(), 6);
        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

        foreach (GameObject moita in moitasVermelhas)
            moita.SetActive(false);
        #endregion

        #region Menina Correndo
        sfx_script.MusicaCombate();
        meninaCorrendo = true;
        foreach (GameObject pos in listaPosMeninaCorrendo)
        {
            proxPosMeninaAux = pos;
            yield return new WaitUntil(() => podeIrProxPos);
            podeIrProxPos = false;
        }
        meninaCorrendo = false;
        #endregion

        //efeitos sonoros tensos, tela escurece rápido, efeito sonoro de garra, volta sprite menina caído, bem mais de noite, menina segue caminho
        #region Tela escurecer, barulho garra, muda sprite, volta bem escuro, 
        sfx_script.PararMusicaCombate();
        luzesCiclo.MudarCorAmbiente(Color.black);
        yield return new WaitForSeconds(.5f);
        sfx_script.GarraCuca();
        yield return new WaitForSeconds(1);
        sfx_script.MeninaCaindo();
        yield return new WaitForSeconds(8);
        //luzesCiclo.MudarCorAmbiente(new Color(0.05f, .07f, .21f), 4f);
        #endregion

        Etapas.CucaSequestro = false;
        SceneManager.LoadScene("T03_comunidade");
    }

    IEnumerator Iniciar_FlorestaEscura()
    {
        JogadorController.Instance.transform.position = posInicialMenina.position;
        oQueFazer_script.GerenciarQuadroQuest_celebr_seq(3);

        JogadorController.Instance.velInicial = 1f;
        JogadorController.Instance.velocidade = 1f;

        #region fade from black
        //EscurecerTela(0);
        //ClarearTela(new Color(.12f, .16f, .5f));
        MudarEstadoJogador(false);
        luzesCiclo.MudarCorAmbiente(Color.black);
        luzesCiclo.MudarCorAmbiente(new Color(.12f, .16f, .5f), .5f);
        yield return new WaitForSeconds(3);
        MudarEstadoJogador(true);
        #endregion

        #region dialogos menina
        JogadorController.Instance.falandoSozinha = true;
        
        for(int i = 0; i < 11; i++)
        {
            JogadorDialogo.Instance.Interagir_CelebracaoCutscene();
            yield return new WaitForSeconds(3);
        }
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
        //JogadorController.Instance.podeFlipX = acao;
        JogadorController.Instance.estaDuranteCutscene = !acao;
    }
}
