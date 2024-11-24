using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteTamanhoCamera : MonoBehaviour
{
    CinemachineVirtualCamera camVirtual;
    bool mudandoTamanhoCam;
    float tamanhoDesejado;
    float tempoDeTransicao; // Tempo total de transição
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

            // Interpolação entre o valor atual e o desejado
            float progressao = Mathf.Clamp01(tempoAtual / tempoDeTransicao); // Garante que não passe de 1
            float novoTamanho = Mathf.Lerp(camVirtual.m_Lens.OrthographicSize, tamanhoDesejado, progressao);

            // Atribui o novo tamanho
            camVirtual.m_Lens.OrthographicSize = novoTamanho;

            // Quando atingir o tamanho desejado, parar a interpolação
            if (progressao >= 1f)
            {
                camVirtual.m_Lens.OrthographicSize = tamanhoDesejado;
                mudandoTamanhoCam = false; // Finaliza a transição
            }
        }
    }

    // Método para ajustar o tamanho da câmera com tempo de transição
    public void AjustarTamanhoCamera(float tamanho = 2f, float tempo = 1f)
    {
        if (camVirtual != null && camVirtual.m_Lens.OrthographicSize != tamanho)
        {
            tamanhoDesejado = tamanho;
            tempoAtual = 0f; // Reseta o contador de tempo
            mudandoTamanhoCam = true;
            tempoDeTransicao = tempo; // Define o tempo total de transição
        }
    }

    public bool ChecarTamanhoCamera(float tamanho)
    {
        return camVirtual.m_Lens.OrthographicSize == tamanho ? true : false;
    }
}
