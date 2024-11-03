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
    bool aconteceuBriga;

    #region NPCS GO
    GameObject velhaNamiaCelebracaoGO;
    #endregion

    #region BRIGA
    [SerializeField] GameObject posMeninaBriga;
    [SerializeField] GameObject posVelhaNamiaBriga;
    bool pessoasAndando;
    #endregion

    private void Awake()
    {
        musicaSource = GetComponent<AudioSource>();
        meninoScript = GameObject.Find("Menino").GetComponent<Menino>();
    }

    private void Start()
    {
        velhaNamiaCelebracaoGO = GameObject.Find("VelhaNamia");

        if (SceneManager.GetActiveScene().name == "01_comunidade")
            StartCoroutine(IniciarJogo_MeninaTocando());
    }

    private void FixedUpdate()
    {
        if(Etapas.BrigaCelebracao && this.gameObject.name == "TriggerBriga" && pessoasAndando)
        {
            JogadorController.Instance.transform.position = Vector2.MoveTowards(JogadorController.Instance.transform.position, posMeninaBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
            velhaNamiaCelebracaoGO.transform.position = Vector2.MoveTowards(velhaNamiaCelebracaoGO.transform.position, posVelhaNamiaBriga.transform.position, JogadorController.Instance.velocidade * Time.deltaTime);
            Debug.Log("andando");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerBriga" && qntdNpcsConversados >= totalNpcsConversaveis)
            StartCoroutine(Brigar());
        if(collision.GetComponent<JogadorController>() && gameObject.name == "TriggerCucaSeq")
            CucaSequestraMenino();
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerPerseguirCuca")
            PerseguirCuca();
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

        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo); // esperar dialogo com menino acabar

        MudarEstadoJogador(true);
        Menino.acabouFalar = true;

        Etapas.MeninaTocandoUkulele = false;
    }

    IEnumerator Brigar()
    {
        Debug.Log("BRIGARRR");
        aconteceuBriga = true;
        Etapas.BrigaCelebracao = true;

        #region Povo se movendo na direção de sua pos da cena
        pessoasAndando = true;
        yield return new WaitForSeconds(3);
        pessoasAndando = false;
        #endregion

        MudarEstadoJogador(false);

        Interagir_Celebracao(velhaNamiaCelebracaoGO.GetComponent<VelhaNamiaCelebracao>(), 0);

        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);
        Debug.Log("mia goth");
        //meninoScript.Interagir_CelebracaoCutscene(0);

        MudarEstadoJogador(true);
        Etapas.BrigaCelebracao = false;
    }

    void Interagir_Celebracao(MonoBehaviour script, int index)
    {
        object[] parametros = { index };

        var metodo = script.GetType().GetMethod($"Interagir_CelebracaoCutscene");
        metodo.Invoke(script, parametros);
    }

    void CucaSequestraMenino()
    {
        if(aconteceuBriga)
            Debug.Log("sequestro");
    }

    void PerseguirCuca()
    {
        Debug.Log("perseguir");
    }

    void MudarEstadoJogador(bool acao)
    {
        JogadorController.Instance.podeAtacar = acao;
        JogadorController.Instance.podeMover = acao;
    }
}
