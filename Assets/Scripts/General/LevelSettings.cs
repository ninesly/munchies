using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public enum Goals { AliveMunchies, TotalSmunchies, Deaths, BlobsEaten, TotalExp, TotalIncubPoints };

    [Header("---- Level Unique Settings")]
    public AudioClip levelMusic;

    public string goalText;
    public Goals goalType;
    public int goalAmount;

    [Header("Progression")]
    public bool turnOffExperience = false;
    [Tooltip("Not dependent from Teaching and vice versa")]
    public bool turnOffRandomEvents = true;
    [Tooltip("Not dependent from Random Events and vice versa")]
    public bool turnOffTeaching = false;
    [Tooltip("Dependent from Teaching. Teaching has to be On")]
    public bool turnOffPassingSmunchieClass = false;
    public bool turnOffIncubationMechanic = false;
    public bool moatherClassEnabled = true;
    public bool dictoClassEnabled = true;
    public bool sacriClassEnabled = true;
    public bool seeerClassEnabled = true;
    public bool whipyClassEnabled = true;

    [Header("Turn on variations")]
    public bool colorVariation = true;
    public bool sizeVariation = true;
    public bool uniqueVariation = true;

    [Header("Blob Spawn")]
    public bool randomPlaceSpawn = true;
    public bool randomBlobTypeSpawn = true;
    public bool randomColorOfBlob = false;

    [Header("Difficulty Settings")]
    [Header("they will add / subtract from Global Settings")]
    [Header("---- Tuning Settings")]
    

    
    public float thislevel_timeBetweenPoisonDamage;
    public int thislevel_fallingDamage;

    [Header("Difficulty - Blob Spawns")]
    public float thislevel_timeOfRemainig;
    public float thislevel_timeOfHint;

    [Header("Munchies Base Settings")]
    public int thislevel_baseHealth;
    public int thislevel_startingLevel;
    [Tooltip("how many exp for level up")]
    public int thislevel_levelThreshold;
    [Tooltip("how levelups to advance")]
    public int thislevel_advancingThreshold;
    public int thislevel_baseTeachingLimit;

    [Header("Incubation")]
    public int thislevel_startingCost;
    [Tooltip("Amount that price will be higher after each purchase")]
    public int thislevel_costsJumps;


    [Header("Event System")]
    public int thislevel_dice;
    [Tooltip("if roll dice <= this value then event will occure")]
    public int thislevel_eventProbability;
    [Tooltip("if roll dice <= this value and > than eventProb then teaching will occure")]
    public int thislevel_teachingProbability;
    [Tooltip("teachingProb can raise more than that")]
    public int thislevel_notMoreThan;
    [Tooltip("only works if teachExpEqualToLevel is false")]
    public int thislevel_expToTeachIfFixed;

    [Header ("Debug Only")]
    public float timeBetweenPoisonDamage = 0;
    public int fallingDamage = 0;
    public float timeOfRemainig = 0;
    public float timeOfHint = 0;
    public int baseHealth = 0;
    public int startingLevel = 0;
    public int levelThreshold = 0;
    public int advancingThreshold = 0;
    public int baseTeachingLimit = 0;
    public int startingCost = 0;
    public int costsJumps = 0;
    public int dice = 0;
    public int eventProbability = 0;
    public int teachingProbability = 0;
    public int notMoreThan = 0;
    public int expToTeachIfFixed = 0;


    List<bool> smunchiesStates;
    CanvasObjectsSwitch statisticsHUD;
    CanvasObjectsSwitch incubatorHUD;
    bool hud_checked = false;

    GlobalSettings globalSettings;
    

    public void ChangeTimeOfHint(float amount)
    {
        timeOfHint += amount;
    }

    private void Awake()
    {
        SetUpAllVars();
    }

    private void Start()
    {
        SetUpMusic();
    }

    private void Update()
    {
        if (!hud_checked && Time.timeScale > 0) TurnOffHUDComponents();
    }

    private void SetUpAllVars()
    {
        globalSettings = FindObjectOfType<GlobalSettings>();

        timeBetweenPoisonDamage =  globalSettings.timeBetweenPoisonDamage + thislevel_timeBetweenPoisonDamage;
        fallingDamage = globalSettings.fallingDamage + thislevel_fallingDamage;
        timeOfRemainig = globalSettings.timeOfRemainig + thislevel_timeOfRemainig;
        timeOfHint = globalSettings.timeOfHint + thislevel_timeOfHint;
        baseHealth = globalSettings.baseHealth + thislevel_baseHealth;
        startingLevel = globalSettings.startingLevel + thislevel_startingLevel;
        levelThreshold = globalSettings.levelThreshold + thislevel_levelThreshold;
        advancingThreshold = globalSettings.advancingThreshold + thislevel_advancingThreshold;
        baseTeachingLimit = globalSettings.baseTeachingLimit + thislevel_baseTeachingLimit;
        startingCost = globalSettings.startingCost + thislevel_startingCost;
        costsJumps = globalSettings.costsJumps + thislevel_costsJumps;
        dice = globalSettings.dice + thislevel_dice;
        eventProbability = globalSettings.eventProbability + thislevel_eventProbability;
        teachingProbability = globalSettings.teachingProbability + thislevel_teachingProbability;
        notMoreThan = globalSettings.notMoreThan + thislevel_notMoreThan;
        expToTeachIfFixed = globalSettings.expToTeachIfFixed + thislevel_expToTeachIfFixed;
    }

    private void SetUpMusic()
    {
        var player = FindObjectOfType<MusicPlayer>();           
        if (!player) return;        
        player.GetComponent<AudioSource>().clip = levelMusic;
        //player.GetComponent<AudioSource>().Play();
    }

    private void TurnOffHUDComponents()
    {
        hud_checked = true;

        statisticsHUD = GameObject.FindGameObjectWithTag("GamePlayStatistics").GetComponent<CanvasObjectsSwitch>();
        incubatorHUD = GameObject.FindGameObjectWithTag("IncubatorHUD").GetComponent<CanvasObjectsSwitch>();

        if (turnOffExperience)
        {
            statisticsHUD.SetActiveCanvaObjectNr(2, false); //expText
        }
        else
        {
            statisticsHUD.SetActiveCanvaObjectNr(2, true); //expText
        }



        
        if (turnOffIncubationMechanic)
        {
            incubatorHUD.SetActiveCanvaObjectNr("all", false);
           // statisticsHUD.SetActiveCanvaObjectNr(3, false);  //incubPoint
        }
        else
        {
            GatherAllSmunchiesStates();

            
          //  statisticsHUD.SetActiveCanvaObjectNr(3, true); //incubPoint

            for (int index = 0; index < smunchiesStates.Count; index++)
            {
                incubatorHUD.SetActiveCanvaObjectNr(index, smunchiesStates[index]);
            }
            
        }
    }

    private void GatherAllSmunchiesStates()
    {
        smunchiesStates = new List<bool>()
        {
            true, //"icub points"
            true, //"icub new smunchie"
            moatherClassEnabled,
            dictoClassEnabled,
            sacriClassEnabled,
            seeerClassEnabled,
            whipyClassEnabled
        };
        /*
        smunchiesStates.Add(moatherClassEnabled);
        smunchiesStates.Add(dictoClassEnabled);
        smunchiesStates.Add(sacriClassEnabled);
        smunchiesStates.Add(seeerClassEnabled);
        smunchiesStates.Add(whipyClassEnabled);*/
    }
}
