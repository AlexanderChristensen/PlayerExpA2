using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MechanismManager : MonoBehaviour
{
    [Header ("Mechanism Totals")]
    public float shipPowerTotal;
    public float oxygenQualityTotal;
    public float sheildsTotal;
    public float shipHealthTotal;
    public float experimentTotal;
    public float totalPowerDraw;

    [HideInInspector] public float shipPower;
    [HideInInspector] public float oxygenQuality;
    [HideInInspector] public float sheilds;
    [HideInInspector] public float shipHealth;
    [HideInInspector] public float experiment;

    [Header("Mechanism Variables")]
    [SerializeField] float sheildRegenMultiplier;
    [SerializeField] float oxygenFilteringMultiplier;
    [SerializeField] float experimentMultiplier;
    [SerializeField] float shipCycleTime;
    [SerializeField] float oxygenLossPerCycle;

    [Header("References")]
    [SerializeField] TMP_Text shipPowerText; 
    [SerializeField] TMP_Text oxygenQualityText; 
    [SerializeField] TMP_Text sheildsText; 
    [SerializeField] TMP_Text shipHealthText;
    [SerializeField] TMP_Text experimentText;

    [SerializeField] string oxygenFilterTerminalName;
    [SerializeField] string sheildsTerminalName;
    [SerializeField] string experimentTerminalName;

    [SerializeField] List<GameObject> terminals = new List<GameObject>();
    [SerializeField] TerminalData hubTerminal;

    float shipTimer;

    float powerDrawTimer;
    float asteroidHitTimer;

    float oxygenFilteringDraw;
    float sheildDraw;
    float experimentDraw;

    void Start()
    {
        shipPower = shipPowerTotal;
        oxygenQuality = oxygenQualityTotal;
        sheilds = sheildsTotal;
        shipHealth = shipHealthTotal;
    }

    void Update()
    {

        foreach (GameObject terminal in terminals)
        {
            if (terminal.name == oxygenFilterTerminalName)
            {
                oxygenFilteringDraw = terminal.GetComponent<TerminalInputControl>().onlinePowerDraw;
            }
            else if (terminal.name == sheildsTerminalName)
            {
                sheildDraw = terminal.GetComponent<TerminalInputControl>().onlinePowerDraw;
            }
            else if (terminal.name == experimentTerminalName)
            {
                experimentDraw = terminal.GetComponent<TerminalInputControl>().onlinePowerDraw;
            }

        }

        shipPowerText.text = "Ship Power: " + shipPower + " out of " + shipPowerTotal;
        oxygenQualityText.text = "O2 Quality: " + oxygenQuality + " out of " + oxygenQualityTotal + " -- " + (oxygenFilteringDraw * oxygenFilteringMultiplier) + "-c regen";
        sheildsText.text = "Shields: " + sheilds + " out of " + sheildsTotal + " -- " + (sheildDraw * sheildRegenMultiplier) + "-c regen";
        shipHealthText.text = "Health: " + shipHealth + " out of " + shipHealthTotal;
        experimentText.text = "Experiment Complete: " + experiment + " out of " + experimentTotal;

        ShipTimer();
        AsteroidHit();
    }

    void ShipTimer()
    {
        if (shipTimer >= shipCycleTime)
        {
            PowerDraw();
            SheildRegen();
            OxygenDecrease();
            OxygenFiltering();
            Experiment();

            shipTimer = 0;
        }
        else
        {
            shipTimer += Time.deltaTime;
        }
    }

    void PowerDraw()
    {
        for (int i = 0; i < hubTerminal.cellBatteryAmount.Count; i++)
        {
            hubTerminal.cellBatteryAmount[i] -= hubTerminal.cellPowerDraw[i];

            if (hubTerminal.cellBatteryAmount[i] <= 0)
            {
                hubTerminal.cellBatteryAmount[i] = 0;
            }
        }

        shipPower = 0;

        foreach (float cell in hubTerminal.cellBatteryAmount)
        {
            shipPower += cell;
        }

        if (shipPower <= 0) 
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void AsteroidHit()
    {
        if (asteroidHitTimer >= Random.Range(3,5))
        {
            if (sheilds > 0)
            {
                sheilds -= Random.Range(1, 3);
                 
                if (sheilds < 0)
                {
                    shipHealth += sheilds;
                    sheilds = 0;
                }
            }
            else
            {
                shipHealth -= Random.Range(1, 3);
            }

            asteroidHitTimer = 0;
        }
        else
        {
            asteroidHitTimer += Time.deltaTime;
        }
    }

    void SheildRegen()
    {
        if (sheilds < sheildsTotal)
        {
            if ((sheilds + sheildDraw * sheildRegenMultiplier) < sheildsTotal)
            {
                sheilds += sheildDraw * sheildRegenMultiplier;
            }
            else
            {
                sheilds = sheildsTotal;
            }
        }
    }

    void OxygenDecrease()
    {
        if (oxygenQuality > 0)
        {
            oxygenQuality -= oxygenLossPerCycle;

            if (oxygenQuality < 0)
            {
                oxygenQuality = 0;
            }
        }
        else
        {
            oxygenQuality = 0;
        }
    }

    void OxygenFiltering()
    {
        if (oxygenQuality < oxygenQualityTotal)
        {
            if ((oxygenQuality + oxygenFilteringDraw * oxygenFilteringMultiplier) < oxygenQualityTotal)
            {
                oxygenQuality += oxygenFilteringDraw * oxygenFilteringMultiplier;
            }
            else
            {
                oxygenQuality = oxygenQualityTotal;
            }
        }
    }

    void Experiment()
    {
        if (experiment < experimentTotal)
        {
            if ((experiment + experimentDraw * experimentMultiplier) < experimentTotal)
            {
                experiment += experimentDraw * experimentMultiplier;
            }
            else
            {
                oxygenQuality = oxygenQualityTotal;
                SceneManager.LoadScene("WinScreen");
            }
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
