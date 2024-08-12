using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidades : MonoBehaviour
{
    public bool escudo; // ok
    public bool atqArea;
    public bool cura; // ok
    public bool tiroFantasma;
    public bool invocar;
    public bool superVelocidade; // ok

    JogadorVida jogadorVida;
    JogadorControls jogadorControls;

    Escudo escudoScript;
    Cura curaScript;
    SuperVelocidade superVelocidadeScript;
    AtaqueArea ataqueAreaScript;
    Invocacao invocacao;

    private void Awake()
    {
        jogadorControls = new JogadorControls();
        escudoScript = GetComponent<Escudo>();
        curaScript = GetComponent<Cura>();
        superVelocidadeScript = GetComponent<SuperVelocidade>();
        ataqueAreaScript = GetComponent<AtaqueArea>();
        invocacao = GetComponent<Invocacao>();
    }

    private void Start()
    {
        //jogadorControls.Combat.Escudo.performed += _ => Escudo();

        jogadorVida = gameObject.GetComponent<JogadorVida>();
    }


    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && escudo)
            escudoScript.CriarEscudo();
        if (Keyboard.current.fKey.wasPressedThisFrame && cura)
            curaScript.Curar();
        if (Keyboard.current.cKey.wasPressedThisFrame && superVelocidade)
            superVelocidadeScript.Velocidade();
        if (Keyboard.current.rKey.wasPressedThisFrame && atqArea)
            ataqueAreaScript.CriarAtaqueArea();
        if (Keyboard.current.rKey.wasPressedThisFrame && invocar)
            invocacao.Invocar();


    }
}
