using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olhos : MonoBehaviour
{
    bool podePiscar;
    bool estaPiscando;
    float intervalo;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        podePiscar = true;
        estaPiscando = false;
    }

    void Update()
    {
        if (podePiscar && !estaPiscando)
            StartCoroutine(PiscarOlhos());
    }

    IEnumerator PiscarOlhos()
    {
        estaPiscando = true;
        intervalo = Random.Range(3, 20);
        //Debug.Log("intervalo = "+intervalo);
        animator.Play("Piscar");
        yield return new WaitForSeconds(intervalo);
        estaPiscando = false;
    }
}
