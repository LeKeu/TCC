using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garca : MonoBehaviour
{
    private enum Estado
    {
        Andando,
        Parado
    }

    [SerializeField] float movVel = 1f;
    [SerializeField] float tempoParado = 2f;
    Vector2 movDirecao;
    Rigidbody2D rb;
    Estado estado;
    float xAntigo;

    // Start is called before the first frame update
    void Awake()
    {
        estado = Estado.Andando;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(Andando());
    }

    void FixedUpdate()
    {
        Vector2 teste = movDirecao * (movVel * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + teste);
    }

    IEnumerator Andando()
    {
        while(estado == Estado.Andando)
        {
            float x = Random.Range(-1f, 1f);
            //float y = Random.Range(-1f, 1f);

            //gameObject.transform.localScale = x*100 < gameObject.transform.position.x ? new Vector2(-1, 1) : new Vector2(1, 1);

            Vector2 andandoPos = new Vector2(x, 1).normalized;
            yield return new WaitForSeconds(tempoParado); 
            IrPara(andandoPos);
        }
    }

    void IrPara(Vector2 posAlvo)
    {
        movDirecao = posAlvo;
        if(movDirecao.x < 0) gameObject.transform.localScale = new Vector2(1, 1);
        else gameObject.transform.localScale = new Vector2(-1, 1);
        Debug.Log(movDirecao);
    }
}
