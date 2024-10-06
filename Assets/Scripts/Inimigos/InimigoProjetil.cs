using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoProjetil : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movDir;
    [SerializeField] float velProj = 2f;
    [SerializeField] float tempoVivo = 4f;

    float auxTempo = 0f;

    public bool seguirJogador;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        auxTempo += Time.deltaTime;

        movDir = (JogadorController.Instance.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + movDir * (velProj * Time.deltaTime));
        //rb.gameObject.transform.Translate(Vector2.right * velProj * Time.deltaTime);

        if (auxTempo >= tempoVivo) { Destroy(this.gameObject); auxTempo = 0f; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Indestrutivel indestrutivel = collision.gameObject.GetComponent<Indestrutivel>();

        if (!collision.isTrigger && indestrutivel || collision.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    //inimigoPathFinding.MoverPara((jogadorPos - transform.position).normalized);
}
