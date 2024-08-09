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

    private void Awake()
    {
        jogadorControls = new JogadorControls();
        escudoScript = GetComponent<Escudo>();
        curaScript = GetComponent<Cura>();
        superVelocidadeScript = GetComponent<SuperVelocidade>();
        ataqueAreaScript = GetComponent<AtaqueArea>();
    }

    private void Start()
    {
        //jogadorControls.Combat.Escudo.performed += _ => Escudo();

        jogadorVida = gameObject.GetComponent<JogadorVida>();
    }


    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            escudoScript.CriarEscudo();
        if (Keyboard.current.fKey.wasPressedThisFrame)
            curaScript.Curar();
        if (Keyboard.current.cKey.wasPressedThisFrame)
            superVelocidadeScript.Velocidade();
        if (Keyboard.current.rKey.wasPressedThisFrame && atqArea)
            ataqueAreaScript.CriarAtaqueArea();

    }
}
