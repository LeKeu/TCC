using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidades : MonoBehaviour
{
    public bool escudo;
    public bool atqArea;
    public bool cura;
    public bool tiroFantasma;
    public bool invocar;
    public bool superVelocidade;

    #region ESCUDO
    [SerializeField] GameObject EscudoObj;
    [SerializeField] private float escudoTempo = 5f;
    #endregion

    JogadorVida jogadorVida;
    JogadorControls jogadorControls;

    private void Awake()
    {
        jogadorControls = new JogadorControls();
    }

    private void Start()
    {
        jogadorControls.Combat.Escudo.performed += _ => Escudo();

        jogadorVida = gameObject.GetComponent<JogadorVida>();
    }


    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            Escudo();
    }

    public void Escudo()
    {
        if (escudo)
        {
            StartCoroutine(EscudoRoutine());
        }
    }

    IEnumerator EscudoRoutine()
    {
        Debug.Log("escudo");
        GameObject escudoOjbInst = Instantiate(EscudoObj, gameObject.transform);
        escudoOjbInst.transform.parent = gameObject.transform;
        jogadorVida.podeLevarDano = false;
        yield return new WaitForSeconds(escudoTempo);
        Destroy(escudoOjbInst);
        jogadorVida.podeLevarDano = true;

    }
}
