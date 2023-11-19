using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraShakeController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCam;
    CinemachineBasicMultiChannelPerlin perlinNoise;
    public bool letPlay = false;
    public GameObject Sparks1;
    public GameObject Sparks2;
    public GameObject Sparks3;
    public GameObject Sparks4;
    public GameObject Sparks5;


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
        letPlay = true;

    }

    IEnumerator WaitTimer(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
        letPlay = false;
    }
    void Update()
    {

        if (letPlay)
        {
            Sparks1.GetComponent<ParticleSystem>().Play();
            Sparks2.GetComponent<ParticleSystem>().Play();
            Sparks3.GetComponent<ParticleSystem>().Play();
            Sparks4.GetComponent<ParticleSystem>().Play();
            Sparks5.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            Sparks1.GetComponent<ParticleSystem>().Stop();
            Sparks2.GetComponent<ParticleSystem>().Stop();
            Sparks3.GetComponent<ParticleSystem>().Stop();
            Sparks4.GetComponent<ParticleSystem>().Stop();
            Sparks5.GetComponent<ParticleSystem>().Stop();
        }

    }
}
