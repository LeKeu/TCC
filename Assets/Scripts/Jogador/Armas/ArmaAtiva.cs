using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmaAtiva : Singleton<ArmaAtiva>
{
    public MonoBehaviour ArmaAtivaAtual { get; private set; }

    private JogadorControls jogadorControls;
    float tempoEntreAtaques;

    bool atacarButBaixo, estaAtacando = false;

    protected override void Awake()
    {
        base.Awake();

        jogadorControls = new JogadorControls();
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "01_comunidade" && SceneManager.GetActiveScene().name != "02_comunidade")
        {
            jogadorControls.Combat.Attack.started += _ => ComecarAtaque();
            jogadorControls.Combat.Attack.canceled += _ => AcabarAtaque();

            AtaqueCoolDown();
        }
        else gameObject.SetActive(false);// se for nas cenas de comunidade, n�o tem como mudar nem utilizar a arma

    }

    private void Update()
    {
        if(JogadorController.Instance.podeAtacar)
            Atacar();
    }

    public void NovaArma(MonoBehaviour novaArma)
    {
        ArmaAtivaAtual = novaArma;

        AtaqueCoolDown();
        tempoEntreAtaques = (ArmaAtivaAtual as IArma).PegarArmaInfo().armaCooldown;
    }

    public void ArmaNull()
    {
        ArmaAtivaAtual = null;
    }

    //public void ToggleEstaAtacando(bool valor)
    //{
    //    estaAtacando = valor;
    //}

    void AtaqueCoolDown()
    {
        estaAtacando = true;
        StopAllCoroutines();
        StartCoroutine(TempoEntreAtaquesRoutine());
    }

    IEnumerator TempoEntreAtaquesRoutine()
    {
        yield return new WaitForSeconds(tempoEntreAtaques);
        estaAtacando = false;
    }

    void ComecarAtaque()
    {
        atacarButBaixo = true;
    }
    void AcabarAtaque()
    {
        atacarButBaixo = false;
    }

    void Atacar()
    {
        if(atacarButBaixo && !estaAtacando)
        {
            //estaAtacando = true;
            AtaqueCoolDown();
            (ArmaAtivaAtual as IArma).Atacar();
        }
    }
}
