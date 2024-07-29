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
    }

    public void TremerCameraFunc()
    {
        StartCoroutine(TremerCamRoutine());
    }

    IEnumerator TremerCamRoutine()
    {
        channelPerlin.m_AmplitudeGain = tremerIntensidade;
        yield return new WaitForSeconds(tremerTempo);
        channelPerlin.m_AmplitudeGain = 0f;
    }


}
