using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class AtqAreaIara : MonoBehaviour
{
    [SerializeField] float tempoVida = 3f;
    float tempoAteDano = 1f;
    [SerializeField] int dano = 4;

    [SerializeField] float alcanceExpl = 2f;

    TremerCamera tremerCamera;
    bool podeDanificar;

    private void Start()
    {
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
        podeDanificar = false;
        StartCoroutine(VidaAtqArea());
    }

    IEnumerator VidaAtqArea()
    {
        yield return new WaitForSeconds(tempoAteDano);

        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, alcanceExpl);
        foreach (Collider2D c in colliders) {
            if (c.GetComponent<JogadorController>()) 
            { c.GetComponent<JogadorVida>().LevarDano(3); tremerCamera.TremerCameraFunc(); }
        }

        yield return new WaitForSeconds(tempoVida-tempoAteDano);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceExpl);
    }
}
