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
    public float velocityAllowance;

    [Header ("Experiment Variables")]
    [SerializeField] float experimentOptimalMulti;
    [SerializeField] float experimentSuboptimalMulti;
    public float subOptimalDeviation;
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

    [HideInInspector] public float oxygenFilteringDraw;
    [HideInInspector] public float sheildDraw;
    [HideInInspector] public float experimentDraw;

    float damageTakenThisCycle;

    float experimentCycleChange;
    [HideInInspector] public float experimentOptimalRange;

    float velocitySampleTimer;
    [HideInInspector] public float lastSampleVelocity;

    float oxygenFilteringRegen;
    float oxygenFilteringDrain;

    bool oxygenDegrading;

    void Start()
    {
        shipPower = shipPowerTotal;
        oxygenQuality = oxygenQualityTotal;
        sheilds = sheildsTotal;
        shipHealth = shipHealthTotal;

        powerUI.SetStartValues(shipPowerTotal, shipPowerTotal, 0f, 0f);
        oxyFltrUI.SetStartValues(oxygenQualityTotal, oxygenQualityTotal, 0f, 0f);
        sheildsUI.SetStartValues(sheildsTotal, sheildsTotal, 0f, 0);
        experimentUI.SetStartValues(experimentTotal, 0, 0f, 0);
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

        powerUI.UpdateValues(shipPower, totalPowerDraw, 0);
        oxyFltrUI.UpdateValues(oxygenQuality, oxygenFilteringDrain, oxygenFilteringRegen);
        sheildsUI.UpdateValues(sheilds, 0,sheildDraw * sheildRegenMultiplier);
        experimentUI.UpdateValues(experiment, 0,experimentDraw * experimentOptimalMulti);

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

        if (hubTerminal.activeCells.Count > 0)
        {
            for (int i = 0; i < hubTerminal.activeCells.Count; i++)
            {
                hubTerminal.cellBatteryAmount[hubTerminal.activeCells[i]] -= hubTerminal.cellPowerDraw[hubTerminal.activeCells[i]];

                //if (hubTerminal.cellBatteryAmount[hubTerminal.activeCells[i]] <= 0)
                //{
                //    hubTerminal.cellBatteryAmount[hubTerminal.activeCells[i]] = 0;
                //    hubTerminal.activeCells.RemoveAt(i);
                //    hubTerminal.batteryCells.RemoveAt(hubTerminal.activeCells[i]);
                //}

                totalPowerDraw -= hubTerminal.cellPowerDraw[hubTerminal.activeCells[i]];
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
                oxygenFilteringRegen = oxygenFilteringDraw * oxygenFilteringMultiplier;
                oxygenQuality += oxygenFilteringRegen;
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
            experimentOptimalRange = Random.Range(2, 8);

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
                oxygenFilteringDrain = 0;
            }
            else
            {
                oxygenFilteringDrain = oxygenLossPerCycle;
                oxygenQuality -= oxygenFilteringDrain;
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
