using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Incubator : MonoBehaviour
{   
    public GameObject smunchieContainer;

    GameObject[] spawnPoints;
    Slider[] sliders;
    StatisticGatherer statisticGatherer;

    private void Start()
    {
        sliders = GetComponentsInChildren<Slider>();
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
    }

    private void Update()
    {
        var currentIP = statisticGatherer.currentIncubationPoints;
        foreach(Slider slider in sliders)
        {
            if (slider.maxValue >= currentIP)
            {
                slider.value = currentIP;
            }
            else
            {
                slider.value = slider.maxValue;
            }            
        }
    }
    public void Incubate(GameObject chosenClass)
    {        
        var newSmunchie = Instantiate(chosenClass, FindSpawnPoint().transform.position, Quaternion.identity);
        FallingFromHeaven(newSmunchie);
    }

    private GameObject FindSpawnPoint()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        var choosenPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        return choosenPoint;
    }

    private void FallingFromHeaven(GameObject newCreation)
    {
        var navAgent = newCreation.GetComponent<NavMeshAgent>();
        var munchie = newCreation.GetComponent<Munchie>();
        if (navAgent) navAgent.enabled = false;
        if (munchie) munchie.Falling(0, false);
    }
}
