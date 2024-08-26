using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparencia : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float qntdTransparencia = .8f;
    [SerializeField] private float sumirTempo = .4f;

    private SpriteRenderer[] spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        {
            //if (spriteRenderer)
            //{
                StartCoroutine(SumirSprite(spriteRenderer, sumirTempo, spriteRenderer[0].color.a, qntdTransparencia));
                if (!JogadorController.Instance.estaSendoPerseguido) JogadorController.Instance.estaEscondido = true;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        {
            //if (spriteRenderer)
            //{
                StartCoroutine(AparecerSprite(spriteRenderer, sumirTempo, spriteRenderer[0].color.a, 1));
                JogadorController.Instance.estaEscondido = false;
            //}
        }
    }

    private IEnumerator SumirSprite(SpriteRenderer[] spriteRenderer, float sumirTempo, float valorInicial, float alvoTransparencia)
    {
        float tempoPassado = 0;
        while(tempoPassado < sumirTempo)
        {
            tempoPassado += Time.deltaTime;
            float novoAlpha = Mathf.Lerp(valorInicial, alvoTransparencia, tempoPassado / sumirTempo);
            foreach(var sr in spriteRenderer)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, novoAlpha);
            
            
            yield return null;
        }
    }

    private IEnumerator AparecerSprite(SpriteRenderer[] spriteRenderer, float sumirTempo, float valorInicial, float alvoTransparencia)
    {
        float tempoPassado = 0;
        while (tempoPassado < sumirTempo)
        {
            tempoPassado += Time.deltaTime;
            float novoAlpha = Mathf.Lerp(valorInicial, alvoTransparencia, tempoPassado / sumirTempo);
            foreach(var sr in spriteRenderer)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, novoAlpha);
            yield return null;
        }
    }

}
