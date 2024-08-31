using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saci : MonoBehaviour
{
    [SerializeField] GameObject bicho;
    [SerializeField] float tempoDeVida = 5f;

    bool primeiraInteracao;

    [SerializeField] List<GameObject> pontosSpawn = new List<GameObject>();
    List<GameObject> invocadosSummonados = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SummonarBichoRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SummonarBichoRoutine()
    {
        SummonarBicho(3);
        yield return new WaitForSeconds(tempoDeVida);
        foreach (GameObject bicho in invocadosSummonados)
            Destroy(bicho);
    }

    void SummonarBicho(int qntd)
    {
        for(int i = 0; i < qntd; i++)
        {
            GameObject bichoSummonado = Instantiate(bicho, pontosSpawn[i].transform);
            invocadosSummonados.Add(bichoSummonado);
        }
    }
}
