using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequnceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> textSegments = new List<GameObject>();
    [SerializeField] GameObject inputPrompt;

    int segmentTracker;
    bool duringDelay;

    void Start()
    {
        StartCoroutine(DelayForInput());
    }

    void Update()
    {
        if (!duringDelay)
        {
            inputPrompt.SetActive(true);
        }
        else
        {
            inputPrompt.SetActive(false);
        }

        if (Input.GetMouseButton(0) && !duringDelay)
        {
            if ((segmentTracker + 1) < textSegments.Count)
            {
                segmentTracker++;
            }
            else
            {
                PlayerPrefs.SetInt("playedTutorial", 1);
                SceneManager.LoadScene("Game");
            }

            StartCoroutine(DelayForInput());
        }

        for (int i = 0; i < textSegments.Count; i++)
        {
            if (i <= segmentTracker)
            {
                textSegments[i].SetActive(true);
            }
        }
    }

    IEnumerator DelayForInput()
    {
        duringDelay = true;

        yield return new WaitForSeconds(3);

        duringDelay = false;
    }
}
