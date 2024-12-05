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
    public bool podeAtacar;

    public bool desativarAux;

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

        jogadorControls = new JogadorControls();
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void Start()
    {
        
        if(SceneManager.GetActiveScene().name == "01_comunidade" || SceneManager.GetActiveScene().name == "02_comunidade")
            podeAtacar = false;
        else podeAtacar = true;

        jogadorControls.Combat.Attack.started += _ => ComecarAtaque();
        jogadorControls.Combat.Attack.canceled += _ => AcabarAtaque();

        AtaqueCoolDown();
    }

    private void Update()
    {
        if (JogadorController.Instance.podeAtacar)
            Atacar();

        if(!desativarAux && !podeAtacar)
        {
            desativarAux = true;
            DesativarArma();
        }
    }

    public void DesativarArma()
    {
        Debug.Log("dsativandooo");
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AtivarArma()
    {
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void NovaArma(MonoBehaviour novaArma)
    {
        if (cenasComArmaDesativada.Contains(SceneManager.GetActiveScene().name))
        {
            DesativarArma();
        }

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
        if (atacarButBaixo && !estaAtacando && podeAtacar)
        {
            //estaAtacando = true;
            AtaqueCoolDown();
            (ArmaAtivaAtual as IArma).Atacar();
        }
    }
}