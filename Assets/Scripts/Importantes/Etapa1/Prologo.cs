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

    #region NPCS SCRIPTS
    VelhaNamiaCelebracao velhaNamiaCelebracaoScript;
    #endregion

    private void Awake()
    {
        musicaSource = GetComponent<AudioSource>();
        meninoScript = GameObject.Find("Menino").GetComponent<Menino>();
    }

    private void Start()
    {
        velhaNamiaCelebracaoScript = GameObject.Find("VelhaNamia").GetComponent<VelhaNamiaCelebracao>();

        if (SceneManager.GetActiveScene().name == "01_comunidade")
            StartCoroutine(IniciarJogo_MeninaTocando());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorController>() && gameObject.name == "TriggerBriga" && qntdNpcsConversados == totalNpcsConversaveis)
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
        Etapas.BrigaCelebracao = true;
        // setar a posição do player fixa com transform, se n delsiza
        // parte de apertar um botao e passar para a proxima parte do dialogo nos npcs
        // (talvez uma função genérica aqui mesmo?? chamando o interagir de cada script necessário de cada gameobject)

        Debug.Log("BRIGARRR");
        MudarEstadoJogador(false);

        aconteceuBriga = true;
        //velhaNamiaCelebracaoScript.Interagir_CelebracaoCutscene(0);
        Interagir_Celebracao(velhaNamiaCelebracaoScript, 0);

        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo);

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
