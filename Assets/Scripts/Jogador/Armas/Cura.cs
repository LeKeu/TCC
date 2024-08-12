using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cura : MonoBehaviour
{
    [SerializeField] int qndtCura = 3;
    [SerializeField] float curaCoolDown = 5f;
    bool estaCurando;

    public void Curar()
    {
        if (!estaCurando)
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
