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

    [SerializeField] float shipCycleTime;


    [Header ("Oxyge Filtering Variuables")]
    [SerializeField] float oxygenFilteringMultiplier;
    [SerializeField] float oxygenLossPerCycle;
    [SerializeField] float velocitySampleRate;
    [SerializeField] float velocityAllowance;

    [Header ("Experiment Variables")]
    [SerializeField] float experimentOptimalMulti;
    [SerializeField] float experimentSuboptimalMulti;
    [SerializeField] float subOptimalDeviation;
    [SerializeField] float cycleToChangeOptimal;

    [Header("References")]
    [SerializeField] TMP_Text shipHealthText;

    [SerializeField] UIMechanismManager powerUI;
    [SerializeField] UIMechanismManager oxyFltrUI;
    [SerializeField] UIMechanismManager sheildsUI;
    [SerializeField] UIMechanismManager experimentUI;

    [SerializeField] string oxygenFilterTerminalName;
    [SerializeField] string sheildsTerminalName;
    [SerializeField] string experimentTerminalName;

    [SerializeField] List<GameObject> terminals = new List<GameObject>();
    [SerializeField] TerminalData hubTerminal;
    [SerializeField] HubScreen hubScreen;

    [SerializeField] GrappleMovement playerMovement;

    float shipTimer;

    float powerDrawTimer;
    float asteroidHitTimer;

    float oxygenFilteringDraw;
    float sheildDraw;
    float experimentDraw;

    float damageTakenThisCycle;

    float experimentCycleChange;
    float experimentOptimalRange;

    float velocitySampleTimer;
    float lastSampleVelocity;

    bool oxygenDegrading;

    void Start()
    {
        shipPower = shipPowerTotal;
        oxygenQuality = oxygenQualityTotal;
        sheilds = sheildsTotal;
        shipHealth = shipHealthTotal;

        powerUI.SetStartValues(shipPowerTotal, shipPowerTotal, 0f);
        oxyFltrUI.SetStartValues(oxygenQualityTotal, oxygenQualityTotal, 0f);
        sheildsUI.SetStartValues(sheildsTotal, sheildsTotal, 0f);
        experimentUI.SetStartValues(experimentTotal, 0, 0f);
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

        shipHealthText.text = "Health: " + shipHealth + " out of " + shipHealthTotal;

        powerUI.UpdateValues(shipPower, totalPowerDraw);
        oxyFltrUI.UpdateValues(oxygenQuality, oxygenLossPerCycle);
        sheildsUI.UpdateValues(sheilds, sheildDraw * sheildRegenMultiplier);
        experimentUI.UpdateValues(experiment, experimentDraw * experimentOptimalMulti);

        ShipTimer();
        AsteroidHit();

        SampleVelocity();
    }

    void ShipTimer()
    {
        if (shipTimer >= shipCycleTime)
        {
            PowerDraw();
            SheildRegen();
            //OxygenDecrease();
            OxygenFiltering();
            Experiment();

            hubScreen.UpdateHubDisplay();

            shipTimer = 0;
        }
        else
        {
            shipTimer += Time.deltaTime;
        }
    }

    void PowerDraw()
    {
        totalPowerDraw = 0;
        for (int i = 0; i < hubTerminal.activeCells.Count; i++)
        {
            hubTerminal.cellBatteryAmount[hubTerminal.activeCells[i]] -= hubTerminal.cellPowerDraw[hubTerminal.activeCells[i]];

            totalPowerDraw -= hubTerminal.cellPowerDraw[hubTerminal.activeCells[i]];
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
                damageTakenThisCycle = Random.Range(3, 8);

                sheilds -= damageTakenThisCycle;
                 
                if (sheilds < 0)
                {
                    shipHealth += sheilds;
                    sheilds = 0;
                }
            }
            else
            {
                shipHealth -= damageTakenThisCycle;

                if (shipHealth <= 0)
                {
                    SceneManager.LoadScene("LoseScreen");
                }
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

    //void OxygenDecrease()
    //{

    //    if (oxygenDegrading)
    //    {
    //        if (oxygenQuality > 0)
    //        {
    //            oxygenQuality -= oxygenLossPerCycle;

    //            if (oxygenQuality < 0)
    //            {
    //                oxygenQuality = 0;
    //            }
    //        }
    //        else
    //        {
    //            oxygenQuality = 0;
    //            SceneManager.LoadScene("LoseScreen");
    //        }
    //    }
    //}

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
        SetOptimalValue();

        Debug.Log("the optimal wattage: " + experimentOptimalRange);

        
        if (experiment < experimentTotal)
        {
            if ((experiment + experimentDraw * experimentOptimalMulti) < experimentTotal)
            {

                if (experimentDraw <= experimentOptimalRange + subOptimalDeviation && experimentDraw >= experimentOptimalRange - subOptimalDeviation)
                {
                    if (experimentDraw == experimentOptimalRange)
                    {
                        experiment += experimentOptimalMulti;
                    }
                    else
                    {
                        experiment += experimentSuboptimalMulti;
                    }
                }
                
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

    void SetOptimalValue()
    {
        if (experimentCycleChange <= 0)
        {
            experimentOptimalRange = Random.Range(3, 7);

            experimentCycleChange = cycleToChangeOptimal;
        }
        else
        {
            experimentCycleChange--;
        }
    }

    void SampleVelocity()
    {
        if (velocitySampleTimer <= 0)
        {
            float velocityDifference = Mathf.Abs(lastSampleVelocity - playerMovement.velocity);

            if (velocityDifference < velocityAllowance)
            {
                Debug.Log("maiantiang velocity");
            }
            else
            {
                oxygenQuality -= oxygenLossPerCycle;
            }

            lastSampleVelocity = playerMovement.velocity;

            velocitySampleTimer = velocitySampleRate;
        }
        else
        {
            velocitySampleTimer -= Time.deltaTime;
        }
    }
}
