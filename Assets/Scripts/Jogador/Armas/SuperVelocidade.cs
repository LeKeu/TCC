using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperVelocidade : MonoBehaviour
{
    [SerializeField] float duracaoSlow = 5f;
    [SerializeField] float velocidadeCooldown = 5f;
    bool estaVelocidade;

    Habilidades habilidades;

    private void Start()
    {
        habilidades = GetComponent<Habilidades>();
        
    }

    public void Velocidade()
    {
        if (habilidades.superVelocidade && !estaVelocidade)
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
