using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invocacao : MonoBehaviour
{
    [SerializeField] GameObject invocarPrefab;
    [SerializeField] float tempoInvocacao = 10f;
    [SerializeField] float collDownInvocacao = 15f;
    bool estaInvocando;
    public void Invocar()
    {
        if (!estaInvocando) { StartCoroutine(InvocarRoutine()); }
    }

    IEnumerator InvocarRoutine()
    {
        estaInvocando = true;
        GameObject invocacao = Instantiate(invocarPrefab, gameObject.transform);
        invocacao.transform.parent = null;
        yield return new WaitForSeconds(tempoInvocacao);
        Destroy(invocacao);
        yield return new WaitForSeconds(collDownInvocacao);
        estaInvocando = false;
    }
}
