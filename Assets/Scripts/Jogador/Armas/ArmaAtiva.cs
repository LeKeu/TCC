using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmaAtiva : Singleton<ArmaAtiva>
{
    public MonoBehaviour ArmaAtivaAtual { get; private set; }

    private JogadorControls jogadorControls;
    float tempoEntreAtaques;

    bool atacarButBaixo, estaAtacando = false;

    HashSet<string> cenasComArmaDesativada = new HashSet<string>
    {
        "01_comunidade",
        "02_comunidade",
        "T03_comunidade", 
        "01_saci"
    };

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(true);
        jogadorControls = new JogadorControls();
        DesativarArma();
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    void DesativarArma()
    {
        if (!cenasComArmaDesativada.Contains(SceneManager.GetActiveScene().name))
        {
            jogadorControls.Combat.Attack.started += _ => ComecarAtaque();
            jogadorControls.Combat.Attack.canceled += _ => AcabarAtaque();

            AtaqueCoolDown();
        }
        else gameObject.SetActive(false);// se for nas cenas de comunidade, não tem como mudar nem utilizar a arma
    }

    public void AtivarArma1(bool acao)
    {
        //Debug.Log("arma ativa 1"+acao);
        Debug.Log("ativarar ma1");

        if (acao)
        {
            jogadorControls.Combat.Attack.started += _ => ComecarAtaque();
            jogadorControls.Combat.Attack.canceled += _ => AcabarAtaque();

            AtaqueCoolDown();
        }
        gameObject.SetActive(acao);
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
