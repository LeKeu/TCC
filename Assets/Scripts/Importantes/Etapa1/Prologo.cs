using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologo : MonoBehaviour
{
    [SerializeField] AudioClip musicaMenina;
    AudioSource musicaSource;
    Menino meninoScript;

    public static int qntdNpcsConversados = 0;
    int totalNpcsConversaveis = 7;
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

        JogadorController.Instance.podeAtacar = false;
        JogadorController.Instance.podeMover = false;

        musicaSource.PlayOneShot(musicaMenina);

        yield return new WaitForSeconds(10); // música tocando por x segundos

        meninoScript.podeMover = true;
        musicaSource.Stop();

        yield return new WaitUntil(() => !meninoScript.estaLonge); // esperar até o menino chegar perto
        meninoScript.Interagir();

        yield return new WaitUntil(() => JogadorController.Instance.acabouDialogo); // esperar dialogo com menino acabar

        JogadorController.Instance.podeAtacar = true;
        JogadorController.Instance.podeMover = true;
        Menino.acabouFalar = true;

        Etapas.MeninaTocandoUkulele = false;
    }

    IEnumerator Brigar()
    {
        aconteceuBriga = true;
        velhaNamiaCelebracaoScript.Interagir_CelebracaoCutscene(0);

        yield return new WaitForSeconds(1);

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
}
