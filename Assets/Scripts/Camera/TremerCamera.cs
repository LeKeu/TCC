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

    private void Start()
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
    public void TremerCameraFuncDinamica(float intensidade, float tempo)
    {
        StartCoroutine(TremerCamRoutineDinamica(intensidade, tempo));
    }

    IEnumerator TremerCamRoutineDinamica(float intensidade, float tempo)
    {
        channelPerlin.m_AmplitudeGain = intensidade;
        yield return new WaitForSeconds(tempo);
        channelPerlin.m_AmplitudeGain = 0f;
    }

    IEnumerator TremerCamRoutine()
    {
        channelPerlin.m_AmplitudeGain = tremerIntensidade;
        yield return new WaitForSeconds(tremerTempo);
        channelPerlin.m_AmplitudeGain = 0f;
    }


}
