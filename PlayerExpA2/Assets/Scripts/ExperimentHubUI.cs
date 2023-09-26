using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperimentHubUI : MonoBehaviour
{
    [SerializeField] MechanismManager mechanismManager;
    [SerializeField] Image diagram;
    [SerializeField] Sprite diagramUnconnected;
    [SerializeField] List<Sprite> diagramSprites = new List<Sprite>();

    [SerializeField] TMP_Text currentWattage;
    [SerializeField] TMP_Text optimalWattage;

    void Update()
    {
        if (mechanismManager.experimentDraw > 0)
        {
            if (mechanismManager.experimentDraw == mechanismManager.experimentOptimalRange)
            {
                diagram.sprite = diagramSprites[0];
            }
            else if (mechanismManager.experimentDraw > mechanismManager.experimentOptimalRange && mechanismManager.experimentDraw <= mechanismManager.experimentOptimalRange + mechanismManager.subOptimalDeviation)
            {
                diagram.sprite = diagramSprites[1];
            }
            else if (mechanismManager.experimentDraw < mechanismManager.experimentOptimalRange && mechanismManager.experimentDraw >= mechanismManager.experimentOptimalRange - mechanismManager.subOptimalDeviation)
            {
                diagram.sprite = diagramSprites[2];
            }
            else
            {
                diagram.sprite = diagramSprites[3];
            }
        }
        else
        {
            diagram.sprite = diagramUnconnected;
        }

        currentWattage.text = "current: " + mechanismManager.experimentDraw;
        optimalWattage.text = "optimal: " + mechanismManager.experimentOptimalRange;
    }
}
