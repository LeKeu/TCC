using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JogadorVida : Singleton<JogadorVida>
{
    //[SerializeField] TremerCamera tremerCamera;
    [SerializeField] int vidaMax = 3;
    [SerializeField] float empurraoValor = 10f;
    [SerializeField] float tempoRecoveryDano = .5f;
    [SerializeField] float tempoHitstop = 0.1f;

    Slider vidaSlider;
    int vidaAtual;

    public bool podeLevarDano = true;
    public static bool estaViva = true;

    Empurrao empurrao;
    Flash flash;
    LuzesCiclo luzesCiclo;
    Respawnar respawnar;

    protected override void Awake()
    {
        base.Awake();
        estaViva = true;

        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
    }

    void Start()
    {
        luzesCiclo = GameObject.Find("Global Light 2D").GetComponent<LuzesCiclo>();
        respawnar = GameObject.Find("RespawnPoint_Script").GetComponent<Respawnar>();
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

    public void LevarDano(int dano)
    {
        if (!podeLevarDano) { return; }

        //tremerCamera.TremerCameraFunc();
        FindObjectOfType<HitStop>()?.hitStop(tempoHitstop);

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
            Morrer();
            //Debug.Log("morreueuue");
        }
    }

    void Morrer() => estaViva = false;

    private IEnumerator RecoveryDanoRoutine()
    {
        yield return new WaitForSeconds(tempoRecoveryDano);
        podeLevarDano = true;
    }
}
