using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilidades : MonoBehaviour
{
    public bool escudo;
    public bool atqArea;
    public bool cura;
    public bool tiroFantasma;
    public bool invocar;

    #region ESCUDO
    [SerializeField] GameObject EscudoObj;
    [SerializeField] private float escudoTempo = 5f;
    #endregion

    JogadorVida jogadorVida;

    private void Start()
    {
        jogadorVida = gameObject.GetComponent<JogadorVida>();
    }


    private void Update()
    {
        
    }

    public void Escudo()
    {
        if (escudo)
        {
            
        }
    }

    IEnumerator EscudoRoutine()
    {
        GameObject escudoOjbInst = Instantiate(EscudoObj, gameObject.transform);
        jogadorVida.podeLevarDano = false;
        yield return new WaitForSeconds(escudoTempo);
        Destroy(escudoOjbInst);
        jogadorVida.podeLevarDano = true;

    }
}
