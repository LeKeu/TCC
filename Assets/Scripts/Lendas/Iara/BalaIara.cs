using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaIara : MonoBehaviour
{
    public float vidaBala = 1f;
    public float rotacao = 0f;
    public float vel = 1f;

    private Vector2 pontoSpawn;
    private float timer = 0f;

    private void Start()
    {
        pontoSpawn = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {
        if (timer > vidaBala) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movimento(timer);
    }

    Vector2 Movimento(float timer)
    {
        float x = timer * vel * transform.right.x;
        float y = timer * vel * transform.right.y;
        return new Vector2(x + pontoSpawn.x, y + pontoSpawn.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            JogadorVida.Instance.LevarDano(2); //checar o dano!
            Destroy(this.gameObject);
        }
    }
}
