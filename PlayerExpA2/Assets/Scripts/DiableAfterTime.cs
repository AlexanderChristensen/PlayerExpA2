using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiableAfterTime : MonoBehaviour
{
    [SerializeField] float delayToDisable;
    void OnEnable()
    {
        StartCoroutine(DelayDisable());
    }

    IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(delayToDisable);

        gameObject.SetActive(false);
    }
}
