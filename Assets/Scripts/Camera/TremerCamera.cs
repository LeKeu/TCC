using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TremerCamera : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    public float tremerIntensidade = 1f;
    public float tremerTempo = 0.2f;

    CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();  
        channelPerlin.m_AmplitudeGain = 0f;
        virtualCamera.Follow = JogadorController.Instance.transform;
    }

    public void TremerCameraFunc()
    {
        StartCoroutine(TremerCamRoutine());
    }
    public void TremerCameraFuncDinamica(float intensidade)
    {
        StartCoroutine(TremerCamRoutine(intensidade));
    }


    IEnumerator TremerCamRoutine(float tremerIntens=1f)
    {
        channelPerlin.m_AmplitudeGain = tremerIntens;
        yield return new WaitForSeconds(tremerTempo);
        channelPerlin.m_AmplitudeGain = 0f;
    }


}
