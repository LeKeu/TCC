using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machucar : MonoBehaviour
{
    [SerializeField] int dano;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            JogadorVida.Instance.LevarDano(dano);
            JogadorVida.Instance.EmpurrarPlayer(collision.gameObject.transform);
            Debug.Log("machucando");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            JogadorVida.Instance.LevarDano(dano);
            JogadorVida.Instance.EmpurrarPlayer(collision.gameObject.transform);
            Debug.Log("machucando");
        }
    }
}
