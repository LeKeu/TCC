using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BOSSRUSH : MonoBehaviour
{
    [SerializeField] GameObject SaciGO;
    [SerializeField] GameObject IaraGO;
    [SerializeField] GameObject CucaGO;

    [SerializeField] GameObject RespawnPoint;

    int vidaAux;
    float velAux=2.5f;

    private void Start()
    {
        SaciGO.SetActive(false);
        IaraGO.SetActive(false);
        CucaGO.SetActive(false);
        //vidaAux = JogadorVida.Instance.vidaMax;
        JogadorVida.vidaAtual = JogadorVida.VIDA_MAXIMA;

        StartCoroutine(BossRush());
    }

    private void Update()
    {
        //if (!JogadorVida.estaViva)
        //{
        //    JogadorVida.Instance.vidaMax = vidaAux;
        //    JogadorVida.vidaAtual = vidaAux;
        //    JogadorController.Instance.velocidade = velAux;
        //    JogadorController.Instance.velInicial = velAux;
        //}
    }

    IEnumerator BossRush()
    {
        yield return new WaitForSeconds(2);

        SaciGO.SetActive(true);
        StartCoroutine(SaciGO.GetComponent<Saci>().SACI_BOSSRUSH());
        yield return new WaitUntil(() => Saci.SACI_BR_FINALIZADO);
        SaciGO.SetActive(false);

        yield return new WaitForSeconds(3);

        IaraGO.SetActive(true);
        StartCoroutine(IaraGO.GetComponent<Iara>().IARA_BOSSRUSH());
        yield return new WaitUntil(() => Iara.IARA_BR_FINALIZADO);
        IaraGO.SetActive(false);

        yield return new WaitForSeconds(3);

        CucaGO.SetActive(true);
        StartCoroutine(CucaGO.GetComponent<Cuca>().CUCA_BOSSRUSH());
        yield return new WaitUntil(() => Cuca.CUCA_BR_FINALIZADO);
        CucaGO.SetActive(false);

        Debug.Log("acabou!");
    }
}
