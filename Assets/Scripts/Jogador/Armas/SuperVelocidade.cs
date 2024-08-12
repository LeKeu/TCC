using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperVelocidade : MonoBehaviour
{
    [SerializeField] float duracaoSlow = 5f;
    [SerializeField] float velocidadeCooldown = 5f;
    bool estaVelocidade;

    public void Velocidade()
    {
        if (!estaVelocidade)
            StartCoroutine(VelocidadeRoutine());
    }

    IEnumerator VelocidadeRoutine()
    {
        estaVelocidade = true;
        Time.timeScale /= 2;
        JogadorController.Instance.velocidade *= 2;
        yield return new WaitForSeconds(duracaoSlow);

        Time.timeScale *= 2;
        JogadorController.Instance.velocidade /= 2;
        yield return new WaitForSeconds(velocidadeCooldown);
        estaVelocidade = false;
    }
}
