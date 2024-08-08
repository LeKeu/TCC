using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegar : MonoBehaviour
{
    private enum PegarTipo
    {
        Moeda,
        Vida
    }

    [SerializeField] private PegarTipo tipo;
    [SerializeField] private float pegarDist = 5f;
    [SerializeField] private float rateAcelerecao = .2f;
    [SerializeField] private float movVel = 3f;

    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float alturaY = 1.5f;
    [SerializeField] private float duracao = 1f;

    private Vector3 movDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(SpawnMoedaRoutine());
    }

    private void Update()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        if(Vector3.Distance(transform.position, jogadorPos) < pegarDist) {
            movDir = (jogadorPos - transform.position).normalized;
            movVel += rateAcelerecao;
        } else {
            movDir = Vector3.zero;
            movVel = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movDir * movVel * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        {
            DetectarTipo();
            Destroy(gameObject);
        }
    }

    private IEnumerator SpawnMoedaRoutine()
    {
        Vector2 posStart = transform.position;
        float randX = transform.position.x + Random.Range(-2f, 2f);
        float randY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 pontoFinal = new Vector2(randX, randY);

        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float linearT = tempo / duracao;
            float alturaT = animCurve.Evaluate(linearT);
            float altura = Mathf.Lerp(0f, alturaY, alturaT);

            transform.position = Vector2.Lerp(posStart, pontoFinal, linearT) + new Vector2(0f, altura);
            yield return null;
        }
    }

    private void DetectarTipo()
    {
        switch (tipo)
        {
            case PegarTipo.Moeda:
                Debug.Log("moeda"); break;
            case PegarTipo.Vida:
                JogadorVida.Instance.CurarPlayer(1);
                Debug.Log("vida"); break;
        }
    }
}
