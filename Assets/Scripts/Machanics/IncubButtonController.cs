using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncubButtonController : MonoBehaviour
{
    [Header("Debug only")]
    public int cost;
    public int numberOfTransactions;
    Button button;
    Slider slider;
    StatisticGatherer statisticGatherer;
    LevelSettings levelSettings;
    GlobalSettings globalSettings;

    private void Start()
    {
        button = GetComponent<Button>();
        slider = GetComponentInChildren<Slider>();
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
        levelSettings = FindObjectOfType<LevelSettings>();
        globalSettings = FindObjectOfType<GlobalSettings>();
        CostCalculation();
    }

    public void CostCalculation()
    {
        if (globalSettings.countOnlyAliveSmunchies)
        {
            cost = levelSettings.startingCost + levelSettings.costsJumps * statisticGatherer.aliveSSSmunchies;
        }
        else
        {
            cost = levelSettings.startingCost + levelSettings.costsJumps * numberOfTransactions;
        }
        slider.maxValue = cost;
    }

    private int ChoosenSmunchieAlivesNumber()
    {
        // maybe in future
        return 0;
    }

    void Update()
    {       
        CostChecker();
    }

    private void CostChecker()
    {
        if (statisticGatherer.currentIncubationPoints >= cost)
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
        }
    }

    public void MakePurchase() // of this specific Smunchie class
    {
        numberOfTransactions++;
        statisticGatherer.ChangeIncubPointsBy(-cost);
    }
}
