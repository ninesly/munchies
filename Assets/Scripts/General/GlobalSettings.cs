using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [Header("Difficulty Settings")]
    public float timeBetweenPoisonDamage = 2;
    public int fallingDamage = 10;

    [Header("Difficulty - Blob Spawns")]
    public float timeOfRemainig = 2;
    public float timeOfHint = 1;

    [Header("Munchies Base Settings")]
    public int baseHealth = 10;
    public int startingLevel = 1;
    [Tooltip("how many exp for level up")]
    public int levelThreshold = 10;
    [Tooltip("how levelups to advance")]
    public int advancingThreshold = 10;
    public int baseTeachingLimit = 2;

    [Header("Incubation")]
    public int startingCost = 5;
    [Tooltip("Amount that price will be higher after each purchase")]
    public int costsJumps = 5;
    [Tooltip("IMPORTANT - it counts ALL the Smunchies")]
    public bool countOnlyAliveSmunchies = false;

    [Header("Event System")]
    public int dice = 8;
    [Tooltip("if roll dice <= this value then event will occure")]
    public int eventProbability = 1;
    [Tooltip("if roll dice <= this value and > than eventProb then teaching will occure")]
    public int teachingProbability = 2;
    [Tooltip("teachingProb can raise more than that")]
    public int notMoreThan = 4;
    [Tooltip("set it to false, and ten set fixed amount")]
    public bool teachExpEqualToLevel = true;
    [Tooltip("only works if teachExpEqualToLevel is false")]
    public int expToTeachIfFixed;

}
