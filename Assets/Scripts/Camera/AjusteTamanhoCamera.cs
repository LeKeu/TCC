using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteTamanhoCamera : MonoBehaviour
{
    CinemachineVirtualCamera camVirtual;
    void Start()
    {
        camVirtual = GetComponent<CinemachineVirtualCamera>();
    }

    public void AjustarTamanhoCamera(float tamanho = 2) // esse � o padr�o. se mudar l� � preciso mudar aqui
    {
        camVirtual.m_Lens.OrthographicSize = tamanho;
    }

    public bool ChecarTamanhoCamera(float tamanho)
    {
        return camVirtual.m_Lens.OrthographicSize == tamanho ? true : false;
    }
}
