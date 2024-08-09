using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cura : MonoBehaviour
{
    [SerializeField] int qndtCura = 3;
    [SerializeField] float curaCoolDown = 5f;
    bool estaCurando;

    Habilidades habilidades;

    private void Start()
    {
        habilidades = GetComponent<Habilidades>();
    }

    public void Curar()
    {
        if (habilidades.cura && !estaCurando)
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
