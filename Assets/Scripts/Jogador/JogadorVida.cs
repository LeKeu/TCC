using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JogadorVida : Singleton<JogadorVida>
{
    [SerializeField] private int vidaMax = 3;
    [SerializeField] private float empurraoValor = 10f;
    [SerializeField] private float tempoRecoveryDano = 1f;

    private Slider vidaSlider;
    private int vidaAtual;
    public bool podeLevarDano = true;
    private Empurrao empurrao;
    private Flash flash;
    TremerCamera tremerCamera;

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
    }

    void Start()
    {
        vidaAtual = vidaMax;
        AtualizarVidaSlider();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        InimigoIA inimigo = collision.gameObject.GetComponent<InimigoIA>();
        InimigoVida inimigoVida = collision.gameObject.GetComponent<InimigoVida>();

        if (inimigo && podeLevarDano && !inimigoVida.estaAtordoado)
        {
            LevarDano(1);
            EmpurrarPlayer(collision.gameObject.transform);
            //StartCoroutine(flash.FlashRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InimigoProjetil inimigoProjetil = collision.gameObject.GetComponent<InimigoProjetil>();

        if(inimigoProjetil && podeLevarDano)
        {
            Destroy(collision.gameObject);
            LevarDano(1);
            EmpurrarPlayer(collision.gameObject.transform);
            //StartCoroutine(flash.FlashRoutine());
        }
    }

    public void EmpurrarPlayer(Transform collision)
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

    public void LevarDano(int dano)
    {
        if (!podeLevarDano) { return; }

        tremerCamera.TremerCameraFunc();

        podeLevarDano = false;
        vidaAtual -= dano;
        StartCoroutine(RecoveryDanoRoutine());
        AtualizarVidaSlider();
        ChecarSePlayerMorreu();
        //checar se o player morreu
    }

    void AtualizarVidaSlider()
    {
        if(vidaSlider == null)
            vidaSlider = GameObject.Find("VidaSlider").GetComponent<Slider>();

        vidaSlider.maxValue = vidaMax;
        vidaSlider.value = vidaAtual;
    }

    void ChecarSePlayerMorreu()
    {
        if(vidaAtual <= 0)
        {
            vidaAtual = 0;
            Debug.Log("morreueuue");
        }
    }

    private IEnumerator RecoveryDanoRoutine()
    {
        yield return new WaitForSeconds(tempoRecoveryDano);
        podeLevarDano = true;
    }
}
