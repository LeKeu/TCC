using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaAtiva : Singleton<ArmaAtiva>
{
    [SerializeField] private MonoBehaviour armaAtivaAtual;
    private JogadorControls jogadorControls;
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
        jogadorControls.Combat.Attack.started += _ => ComecarAtaque();
        jogadorControls.Combat.Attack.canceled += _ => AcabarAtaque();
    }

    private void Update()
    {
        Atacar();
    }

    public void ToggleEstaAtacando(bool valor)
    {
        estaAtacando = valor;
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
            estaAtacando = true;
            (armaAtivaAtual as IArma).Atacar();
        }
    }
}
