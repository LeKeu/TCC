using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JogadorVida : Singleton<JogadorVida>
{
    //[SerializeField] TremerCamera tremerCamera;
    public const int VIDA_MAXIMA = 30;

    public int vidaMax = 30;
    [SerializeField] float empurraoValor = 10f;
    [SerializeField] float tempoRecoveryDano = .5f;
    [SerializeField] float tempoHitstop = 0.1f;

    Slider vidaSlider;
    public static int vidaAtual;

    public bool podeLevarDano = true;
    public static bool estaViva = true;
    public static bool levouDano;

    Empurrao empurrao;
    Flash flash;

    protected override void Awake()
    {
        base.Awake();
        estaViva = true;

        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
    }

    void Start()
    {
        vidaAtual = vidaMax;
        podeLevarDano = true;
        AtualizarVidaSlider();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        InimigoIA inimigo = collision.gameObject.GetComponent<InimigoIA>();
        InimigoVida inimigoVida = collision.gameObject.GetComponent<InimigoVida>();

        if (inimigo && podeLevarDano && !inimigoVida.estaAtordoado)
        {
            LevarDano(1, collision.gameObject.transform);
            //EmpurrarPlayer(collision.gameObject.transform);
            //StartCoroutine(flash.FlashRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InimigoProjetil inimigoProjetil = collision.gameObject.GetComponent<InimigoProjetil>();

        if (inimigoProjetil && podeLevarDano)
        {
            LevarDano(inimigoProjetil.Dano, collision.gameObject.transform);
            if(inimigoProjetil) Destroy(collision.gameObject);
            //EmpurrarPlayer(collision.gameObject.transform);
        }
    }

    public void EmpurrarPlayer(Transform collision, float empurrarValorFunc = 10f)
    {
        empurrao.SerEmpurrado(collision, empurraoValor);
        StartCoroutine(flash.FlashRoutine());
    }

    public void CurarPlayer(int valor)
    {
        if(vidaAtual < vidaMax)
        {
            vidaAtual += 1;
            AtualizarVidaSlider();
        }
    }

    public void LevarDano(int dano, Transform collision = null)
    { // passar a colocar o trasnforms dos inimigos qnd dar dano no player!
        if (!podeLevarDano) { return; }
        levouDano = true;
        if(collision != null) EmpurrarPlayer(collision.gameObject.transform);

        //tremerCamera.TremerCameraFunc();
        FindObjectOfType<HitStop>()?.hitStop(tempoHitstop);

        podeLevarDano = false;
        vidaAtual -= dano;
        Debug.Log("dano="+dano);
        Debug.Log("vidaatual="+vidaAtual);
        StartCoroutine(RecoveryDanoRoutine());
        AtualizarVidaSlider();
        ChecarSePlayerMorreu();
        //checar se o player morreu
    }

    void AtualizarVidaSlider()
    {
        if(vidaSlider == null)
        {
            Debug.Log("vidaslider era null");
            vidaSlider = GameObject.Find("VidaSlider").GetComponent<Slider>();
        }

        vidaSlider.maxValue = vidaMax;
        vidaSlider.value = vidaAtual;

        Debug.Log("vidaSlider.maxValue="+ vidaSlider.maxValue);
        Debug.Log("vidaSlider.value=" + vidaSlider.value);
    }

    void ChecarSePlayerMorreu()
    {
        if(vidaAtual <= 0)
        {
            vidaAtual = 0;
            Morrer();
            //Debug.Log("morreueuue");
        }
    }

    void Morrer() => estaViva = false;

    private IEnumerator RecoveryDanoRoutine()
    {
        yield return new WaitForSeconds(tempoRecoveryDano);
        podeLevarDano = true;
        levouDano = false;
    }
}
