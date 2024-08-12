using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    [SerializeField] GameObject EscudoObj;
    [SerializeField] private float escudoTempo = 5f;
    [SerializeField] float escudoCoolDown = 5f;
    bool escudoAtivo;

    JogadorVida jogadorVida;

    private void Start()
    {
        jogadorVida = GetComponent<JogadorVida>();
    }

    public void CriarEscudo()
    {
        if (!escudoAtivo)
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
}
