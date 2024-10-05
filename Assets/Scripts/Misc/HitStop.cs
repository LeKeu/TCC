using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static IEnumerator hitStop(float tempo)
    {
        Debug.Log("HITSTOP");
        Time.timeScale = 0;
        yield return new WaitForSeconds(tempo);
        Time.timeScale = 1;
    }

}
