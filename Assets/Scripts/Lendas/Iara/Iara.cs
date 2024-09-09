using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iara : MonoBehaviour
{
    public enum Estado
    {
        Distante,
        Centro
    }

    Estado estado;

    Rigidbody2D rb;

    BarraVidaBosses barraVidaBosses;

    [SerializeField] int Vida;
    [SerializeField] List<GameObject> PosLagoFunda;

    bool Boss1;
    int posAnterior = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        estado = Estado.Distante;
        Boss1 = true;

        StartCoroutine(MudarLagoFundo());
    }

    IEnumerator MudarLagoFundo()
    {
        float tempoAle;
        while (estado == Estado.Distante)
        {
            Teletransportar();
            tempoAle = Random.Range(3, 11);
            Debug.Log(tempoAle);
            yield return new WaitForSeconds(tempoAle);
        }
    }

    void Teletransportar()
    {
        int pos = Random.Range(0, 4);

        if (pos == posAnterior)
            pos += pos + 1 > 3 ? -1 : 1;

        gameObject.transform.position = PosLagoFunda[pos].transform.position;
        posAnterior = pos;
    }
}
