using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteTamanhoCamera : MonoBehaviour
{
    CinemachineVirtualCamera camVirtual;
    bool mudandoTamanhoCam;
    float tamanhoDesejado;
    float tempoDeTransicao; // Tempo total de transi��o
    float tempoAtual = 0f;

    void Start()
    {
        camVirtual = GetComponent<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        if (mudandoTamanhoCam)
        {
            // Aumenta o tempo atual com o tempo real (Time.deltaTime)
            tempoAtual += Time.deltaTime;

            // Interpola��o entre o valor atual e o desejado
            float progressao = Mathf.Clamp01(tempoAtual / tempoDeTransicao); // Garante que n�o passe de 1
            float novoTamanho = Mathf.Lerp(camVirtual.m_Lens.OrthographicSize, tamanhoDesejado, progressao);

            // Atribui o novo tamanho
            camVirtual.m_Lens.OrthographicSize = novoTamanho;

            // Quando atingir o tamanho desejado, parar a interpola��o
            if (progressao >= 1f)
            {
                camVirtual.m_Lens.OrthographicSize = tamanhoDesejado;
                mudandoTamanhoCam = false; // Finaliza a transi��o
            }
        }
    }

    // M�todo para ajustar o tamanho da c�mera com tempo de transi��o
    public void AjustarTamanhoCamera(float tamanho = 2f, float tempo = 1f)
    {
        if (camVirtual != null && camVirtual.m_Lens.OrthographicSize != tamanho)
        {
            tamanhoDesejado = tamanho;
            tempoAtual = 0f; // Reseta o contador de tempo
            mudandoTamanhoCam = true;
            tempoDeTransicao = tempo; // Define o tempo total de transi��o
        }
    }

    public bool ChecarTamanhoCamera(float tamanho)
    {
        return camVirtual.m_Lens.OrthographicSize == tamanho ? true : false;
    }
}
