using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool esperando;
    public void hitStop(float duracao)
    {
        if (esperando)
            return;
        //Debug.Log("HITSOP "+ duracao);
        Time.timeScale = 0;
        StartCoroutine(Esperar(duracao));
    }

    IEnumerator Esperar(float duracao)
    {
        esperando = true;
        yield return new WaitForSecondsRealtime(duracao);
        Time.timeScale = 1;
        esperando = false;
    }

}
