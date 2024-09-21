using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtqAreaIara : MonoBehaviour
{
    [SerializeField] float tempoVida = 3f;
    [SerializeField] float tempoAteDano = 2f;
    [SerializeField] int dano = 4;

    TremerCamera tremerCamera;
    bool podeDanificar;

    private void Start()
    {
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
        StartCoroutine(VidaAtqArea());
    }

    IEnumerator VidaAtqArea()
    {
        yield return new WaitForSeconds(tempoAteDano);
        podeDanificar = true;
        yield return new WaitForSeconds(tempoVida);
        podeDanificar = false;
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<JogadorVida>() && podeDanificar)
        {
            collision.GetComponent<JogadorVida>().LevarDano(dano);
            tremerCamera.TremerCameraFunc();
            Debug.Log("ACERTOU");
        }
    }
}
