using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossConditionText : MonoBehaviour
{
    [SerializeField] GameObject oxygenLoss;
    [SerializeField] GameObject sheildLoss;

    void Start()
    {
        if (PlayerPrefs.GetString("LossState") == "oxygen")
        {
            sheildLoss.SetActive(false);
            PlayerPrefs.DeleteKey("LossState");
        }
        else if (PlayerPrefs.GetString("LossState") == "sheild")
        {
            oxygenLoss.SetActive(false);
            PlayerPrefs.DeleteKey("LossState");
        }
    }
}
