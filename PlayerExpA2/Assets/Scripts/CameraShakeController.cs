using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCam;
    CinemachineBasicMultiChannelPerlin perlinNoise;

    void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ResetIntensity();
    }

    public void ShakeCamera(float intesnity, float shakeTime)
    {
        perlinNoise.m_AmplitudeGain = intesnity;
        StartCoroutine(WaitTimer(shakeTime));

    }

    IEnumerator WaitTimer(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }
}
