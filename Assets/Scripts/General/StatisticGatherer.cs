using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticGatherer : MonoBehaviour
{
    public Text aliveMunchiesText;
    public Text deathsText;
    public Text totalExpGatheredText;
    public Text currentIncubationPointsText;
    public Text currentGoalText;
    public Slider goalProgressSlider;

    [Header("Debug only")]

    [Header("Demography")]
    public int aliveMunchies;
    public int totalMunchies;
    public int aliveSSSmunchies;
    public int totalSSSmunchies;

    [Header("Points")]
    public int totalExpGathered;
    public int totalIncubationPoints;
    public int currentIncubationPoints;
    public int incubBonus;

    [Header("Activities")]
    public int deaths;
    public int totalEatenBlobs;

    [Header("Munching Crowd Control")]
    public int choirLeader;
    public int laudMunchies;
    public int mediumMunchies;
    public int quietMunchies;

    [Header("Demography - Detailed")]
    public int aliveClass_Dicto;
    public int totalClass_Dicto;
    public int aliveClass_Sacri;
    public int totalClass_Sacri;
    public int aliveClass_Moather;
    public int totalClass_Moather;
    public int aliveClass_Seeer;
    public int totalClass_Seeer;
    public int aliveClass_Whipy;
    public int totalClass_Whipy;

    //[Header("Blobs")]
    //public int spawnBlob;
    //public int bombBlob;
    //public int electricBlob;
    //public int killerBlob;
    //public int poisonBlob;
    //public int slugBlob;
    //public int thunderBlob;

    LevelController levelController;
    LevelSettings levelSettings;
    SoundController soundController;

    float previousGoalPoint;


    private void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        levelSettings = FindObjectOfType<LevelSettings>();
        soundController = FindObjectOfType<SoundController>();

        previousGoalPoint = goalProgressSlider.value;
    }

    private void Update()
    {
        GoalChecker();
        TextsUpdates();
        SliderAdjustment();
    }

    private void GoalChecker()
    {
        if (goalProgressSlider.value == levelSettings.goalAmount)
        {
            goalProgressSlider.gameObject.SetActive(false);
            levelController.LevelCompleted();
        } else if (aliveMunchies == 0)
        {
            levelController.GameOver();
        }
    }

    private void TextsUpdates()
    {
        aliveMunchiesText.text = "Alive Munchies: " + aliveMunchies;
        deathsText.text = "Death Counter: " + deaths;
        totalExpGatheredText.text = "Total Exp: " + totalExpGathered;
        currentIncubationPointsText.text = "Avaible Incubation Points: " + currentIncubationPoints;
        currentGoalText.text = levelSettings.goalText;
    }

    private void SliderAdjustment()
    {
        goalProgressSlider.maxValue = levelSettings.goalAmount;
        
        if (levelSettings.goalType == LevelSettings.Goals.AliveMunchies) { goalProgressSlider.value = aliveMunchies; }
        else if (levelSettings.goalType == LevelSettings.Goals.BlobsEaten) { goalProgressSlider.value = totalEatenBlobs; }
        else if (levelSettings.goalType == LevelSettings.Goals.Deaths) { goalProgressSlider.value = deaths; }
        else if (levelSettings.goalType == LevelSettings.Goals.TotalSmunchies) { goalProgressSlider.value = totalSSSmunchies; }
        else if (levelSettings.goalType == LevelSettings.Goals.TotalExp) { goalProgressSlider.value = totalExpGathered; }
        else if (levelSettings.goalType == LevelSettings.Goals.TotalIncubPoints) { goalProgressSlider.value = totalIncubationPoints; }

        // sound
        if (goalProgressSlider.value > previousGoalPoint && Time.timeScale > 0)
        {
            soundController.GoalPointSound();
            previousGoalPoint = goalProgressSlider.value;
        }
        else
        {
            previousGoalPoint = goalProgressSlider.value;
        }
    }


    public void ChangeMunchiesNumberBy(int amount)
    {
        aliveMunchies += amount;
        if (amount > 0) totalMunchies += amount;
    }



    public void ChangeDeathsBy(int amount)
    {
        deaths += amount;
    }

    public void ChangeExpBy(int amount)
    {
        totalExpGathered += amount;
    }
    public void ChangeIncubPointsBy(int amount)
    {
        totalEatenBlobs++;
        currentIncubationPoints += amount;
        if(amount > 0) totalIncubationPoints += amount;
    }

    public void ChangeSpecificTypeOfEatenBlob(int amount)
    {

    }

    public void ChangeSmunchiesNumberBy(int amount)
    {
        aliveSSSmunchies += amount;
        if (amount > 0) totalSSSmunchies += amount;
    }

    // Munching Crowd Control

    public void ChangeChoirLeader(int amount)
    {
        choirLeader += amount;
    }

    public void ChangeLaudMunchies(int amount)
    {
        laudMunchies += amount;
    }

    public void ChangeMediumMunchies(int amount)
    {
        mediumMunchies += amount;
    }

    public void ChangeQuietMunchies(int amount)
    {
        quietMunchies += amount;
    }

    public void ChangeNumberOfSmunchieClass(Smunchie.SmunchieClass smunchieClass, int amount) 
    {
        if (smunchieClass == Smunchie.SmunchieClass.Moather)
        {
            aliveClass_Moather += amount;
            if (amount > 0) totalClass_Moather += amount;
        }
        else if (smunchieClass == Smunchie.SmunchieClass.Dicto)
        {
            aliveClass_Dicto += amount;
            if (amount > 0) totalClass_Dicto += amount;
        }
        else if (smunchieClass == Smunchie.SmunchieClass.Sacri)
        {
            aliveClass_Sacri += amount;
            if (amount > 0) totalClass_Sacri += amount;
        }
        else if (smunchieClass == Smunchie.SmunchieClass.Seeer)
        {
            aliveClass_Seeer += amount;
            if (amount > 0) totalClass_Seeer += amount;
        }
        else if (smunchieClass == Smunchie.SmunchieClass.Whipy)
        {
            aliveClass_Whipy += amount;
            if (amount > 0) totalClass_Whipy += amount;
            

            /*
            if (aliveClass_Whipy < 1)
            {
               SettingSpeeder(false);
                alreadyChecked = false;
            } else if (aliveClass_Whipy >= 1 && !alreadyChecked)
            {
                SettingSpeeder(true);
                alreadyChecked = true;
            }*/
        }
    }

    public void ChangeNumberOfIncubBonus(int amount)
    {
        incubBonus += amount;
    }

   /* private void SettingSpeeder(bool state)
    {
        MotionController[] allAliveMunchies = FindObjectsOfType<MotionController>();

        foreach (MotionController mController in allAliveMunchies)
        {
            mController.SetSpeeding(state);
        }

    }  */  

}



/*
 *         for (int goalTypesTotalNumber = 6; goalTypesTotalNumber > 0; goalTypesTotalNumber--) // update if you add a new goal to enums!!!!
        {
            if (goalType == Goals.) 
            {

            }
        }*/