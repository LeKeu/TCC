using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] float escudoCoolDown = 5f;
    bool escudoAtivo;
    #endregion

    #region ATAQUE EM ÁREA
    [SerializeField] GameObject artAreaPrefab;
    [SerializeField] float atqAreaTempo = 2f;
    [SerializeField] int danoAtq;
    [SerializeField] int AtqAreaCoolDown;
    [SerializeField] CircleCollider2D circleCollider;
    #endregion

    #region CURA
    [SerializeField] int qndtCura = 3;
    [SerializeField] float curaCoolDown = 5f;
    bool estaCurando;
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
        if (Keyboard.current.fKey.wasPressedThisFrame)
            Curar();

    }

    public void Escudo()
    {
        if (escudo && !escudoAtivo)
        {
            StartCoroutine(EscudoRoutine());
        }
    }

    IEnumerator EscudoRoutine()
    {
        Debug.Log("escudo");
        escudoAtivo = true;
        GameObject escudoOjbInst = Instantiate(EscudoObj, gameObject.transform);
        escudoOjbInst.transform.parent = gameObject.transform;
        jogadorVida.podeLevarDano = false;

        yield return new WaitForSeconds(escudoTempo);

        Destroy(escudoOjbInst);
        jogadorVida.podeLevarDano = true;

        yield return new WaitForSeconds(escudoCoolDown);

        escudoAtivo = false;
        Debug.Log("Escudo Liberado");
    }

    public void Curar()
    {
        if (cura && !estaCurando)
            StartCoroutine(CuraRoutine());
    }

    IEnumerator CuraRoutine()
    {
        JogadorVida.Instance.CurarPlayer(qndtCura);
        estaCurando = true;
        yield return new WaitForSeconds(curaCoolDown);
        estaCurando = false;
    }
}
